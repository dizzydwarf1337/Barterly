using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class ConfirmEmailDto
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
