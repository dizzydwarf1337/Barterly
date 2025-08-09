using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long")]
    [MaxLength(50, ErrorMessage = "First name must not exceed 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).*$",
        ErrorMessage =
            "Password must contain at least one uppercase letter, one digit, and one special character.")]
    public required string Password { get; set; }
}