using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginExternalResponse> LoginWithGmail(string token, CancellationToken cancToken);
    Task LoginWithFaceBook();
}