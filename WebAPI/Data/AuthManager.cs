using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class AuthManager : IAuthManager
    {
        private UserApi _user;
        private readonly IMapper _mapper;
        private readonly UserManager<UserApi> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<UserApi> _passwordHasher;

        public AuthManager(IMapper mapper, UserManager<UserApi> userManager, IConfiguration configuration, IPasswordHasher<UserApi> passwordHasher)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<IdentityError>> Register(UserApiDto userDto)
        {
            var user = _mapper.Map<UserApi>(userDto);

            var identityResult = IdentityResult.Failed(new IdentityError
            {
                Description = "User sudah tersedia"
            });

            Random rnd = new Random();
            string username = userDto.Email;
            user.UserName = username.Split('@')[0] + rnd.Next(50);

            var cekEmail = await _userManager.FindByEmailAsync(user.Email);
            if (cekEmail == null)
            {
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userDto.role);
                }

                return result.Errors;
            }
            else
            {
                return (IEnumerable<IdentityError>)identityResult;

            }


        }

        public async Task<IEnumerable<IdentityError>> Update(string id, UserApiDto updateUser)
        {
            //var user = _mapper.Map<UserApi>(updateUser);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            user.Email = updateUser.Email;
            user.FullName = updateUser.FullName;
            Random rnd = new Random();
            string username = updateUser.Email;
            user.UserName = username.Split('@')[0] + rnd.Next(50);
            user.ProfileUrl = updateUser.ProfileUrl;
            user.PasswordHash = _passwordHasher.HashPassword(user, updateUser.Password);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                var newRole = updateUser.role;
                await _userManager.AddToRoleAsync(user, newRole);

            }

            return result.Errors;

        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (user == null || isValidUser == false)
            {
                return null;
            }

            var token = await GenerateToken(user);


            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email
            };
        }

        public async Task<IList<string>> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }



        private async Task<string> GenerateToken(UserApi user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration
                ["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims).Union(roleClaims);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
