namespace KatalogProduk.Models
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
