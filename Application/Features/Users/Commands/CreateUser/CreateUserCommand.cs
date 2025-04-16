using API.Core.ApiResponse;
using Application.DTOs.Auth;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<ApiResponse<Unit>>
    {
        public RegisterDto registerDto;
    }
}
