namespace Shop_web_API.Models.DTOs
{
    public class RegisterDTO
    {
            public string Email { get; set; }
            public string Password { get; set; }
            public string? Role { get; set; }
    }
}
