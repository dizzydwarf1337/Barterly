using Application.Core.ApiResponse;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.Public.Accounts.ResendEmailConfirm;

public class ResendEmailConfirmCommandHandler : IRequestHandler<ResendEmailConfirmCommand, ApiResponse<Unit>>
{
    private readonly IMailService _mailService;

    public ResendEmailConfirmCommandHandler(IMailService mailService)
    {
        _mailService = mailService;
    }

    public async Task<ApiResponse<Unit>> Handle(ResendEmailConfirmCommand request,
        CancellationToken cancellationToken)
    {
        await _mailService.SendConfiramationMail(request.Email, cancellationToken);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}