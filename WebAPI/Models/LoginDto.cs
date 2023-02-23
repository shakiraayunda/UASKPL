using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).*$", ErrorMessage = "Password must contain at least one lowercase, one uppercase and one non-letter or digit character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
