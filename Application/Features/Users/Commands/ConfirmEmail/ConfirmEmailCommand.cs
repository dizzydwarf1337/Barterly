using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ApiResponse<Unit>>
    {
        public string userMail { get; set; }
        public string token { get; set; }
    }
}
