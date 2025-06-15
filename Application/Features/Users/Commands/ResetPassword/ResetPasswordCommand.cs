using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ApiResponse<Unit>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Token { get; set; }
    }
}
