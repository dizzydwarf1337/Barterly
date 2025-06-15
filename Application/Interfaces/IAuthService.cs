using Application.DTOs.Auth;
using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginExternalResponse> LoginWithGmail(string token);
        Task LoginWithFaceBook();
    }
}
