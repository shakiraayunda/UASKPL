using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class UserApi : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfileUrl { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile ProfileFile { get; set; }
    }
}
