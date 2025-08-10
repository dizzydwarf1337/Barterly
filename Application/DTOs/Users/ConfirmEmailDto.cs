using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Users;

public class ConfirmEmailDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Token is required")]
    public required string Token { get; set; }
}