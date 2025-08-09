namespace Application.DTOs.Users;

public class ResetPasswordDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Token { get; set; }
}