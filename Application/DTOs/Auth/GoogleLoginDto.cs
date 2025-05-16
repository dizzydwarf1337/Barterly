using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class GoogleLoginDto
    {
        [Required(ErrorMessage ="Token is required")]
        [MinLength(10,ErrorMessage = "Token is too short")]
        public string token { get; set; }
    }
}
