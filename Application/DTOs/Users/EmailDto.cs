using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Users;

public class EmailDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public required string Email { get; set; }
}