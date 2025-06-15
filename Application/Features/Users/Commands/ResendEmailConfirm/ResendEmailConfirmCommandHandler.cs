using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Enums.Common;
using MediatR;

namespace Application.Features.Users.Commands.ResendEmailConfirm
{
    public class ResendEmailConfirmCommandHandler : IRequestHandler<ResendEmailConfirmCommand, ApiResponse<Unit>>
    {
        private readonly IMailService _mailService;

        public ResendEmailConfirmCommandHandler(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task<ApiResponse<Unit>> Handle(ResendEmailConfirmCommand request, CancellationToken cancellationToken)
        {
                await _mailService.SendConfiramationMail(request.Email);
                return ApiResponse<Unit>.Success(value: Unit.Value);
            
        }
    }
}
