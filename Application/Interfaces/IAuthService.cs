using Application.DTOs.Auth;
using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task<UserDto> LoginWithGmail(string token);
        Task Register(RegisterDto registerDto);
        Task LoginWithFaceBook();
        Task LogOut(string userMail);
    }
}
