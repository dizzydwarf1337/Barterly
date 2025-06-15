

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6)]
        public required string Password { get; set; }
    }
}
