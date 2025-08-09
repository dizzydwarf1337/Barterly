using Application.Core.ApiResponse;
using Application.Events.Users.EmailConfirmed;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Public.Accounts.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;

    public ConfirmEmailCommandHandler(UserManager<User> userManager, ILogService logService, IMediator mediator)
    {
        _userManager = userManager;
        _logService = logService;
        _mediator = mediator;
    }

    public async Task<ApiResponse<Unit>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.userMail);
        var confirmResult = await _userManager.ConfirmEmailAsync(user!, request.token);
        await _logService.CreateLogAsync($"User {user!.Email} confirmed email", cancellationToken,
            LogType.Information, userId: user.Id);
        await _mediator.Publish(new EmailConfirmedEvent { Email = request.userMail });
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}