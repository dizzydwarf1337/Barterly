using Application.DTOs.Users;

namespace Application.DTOs.Auth;

public class LoginExternalResponse
{
    public required UserDto UserDto { get; set; }
    public required bool IsFirstTime { get; set; } = false;
}