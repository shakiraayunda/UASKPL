using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KatalogProduk.Models
{
    public class UserApiDto
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfileUrl { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile ProfileFile { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).*$", ErrorMessage = "Password must contain at least one lowercase, one uppercase and one non-letter or digit character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string role { get; set; } = string.Empty;
    }
}
