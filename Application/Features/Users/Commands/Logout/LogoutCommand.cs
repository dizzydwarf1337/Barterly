using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Users.Commands.Logout
{
    public class LogoutCommand : IRequest<ApiResponse<Unit>>
    {
        public string token { get; set; }
    }
}
