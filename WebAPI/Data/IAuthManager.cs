using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Data
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserApiDto userDto);

        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<IList<string>> GetUserRoles(string email);

        Task<IEnumerable<IdentityError>> Update(string id, UserApiDto updateUser);
    }
}
