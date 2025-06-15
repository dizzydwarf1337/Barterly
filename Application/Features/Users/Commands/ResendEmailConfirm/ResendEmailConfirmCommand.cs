using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Users.Commands.ResendEmailConfirm
{
    public class ResendEmailConfirmCommand : IRequest<ApiResponse<Unit>>
    {
        public required string Email { get; set; }
    }
}
