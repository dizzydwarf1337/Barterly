using API.Core.ApiResponse;
using Application.DTOs.Auth;
using Application.DTOs.User;
using MediatR;

namespace Application.Features.Users.Commands.LoginWithGoogle
{
    public class LoginWithGoogleCommand : IRequest<ApiResponse<UserDto>>
    {
        public GoogleLoginDto googleLoginDto { get; set; }
    }
}
