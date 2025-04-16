using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ApiResponse<Unit>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
