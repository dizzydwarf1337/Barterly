using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ApiResponse<Unit>>
    {
        public required string userMail { get; set; }
        public required string token { get; set; }
    }
}
