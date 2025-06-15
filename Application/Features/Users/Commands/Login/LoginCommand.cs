using API.Core.ApiResponse;
using Application.DTOs.Auth;
using Application.DTOs.User;
using MediatR;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse<UserDto>>
    {
        public required LoginDto loginDto { get; set; }
    }
}
