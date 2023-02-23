using Microsoft.AspNetCore.Identity;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserApiDto userDto);

        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<IList<string>> GetUserRoles(string email);

        Task<IEnumerable<IdentityError>> Update(string id, UserApiDto updateUser);
    }
}
