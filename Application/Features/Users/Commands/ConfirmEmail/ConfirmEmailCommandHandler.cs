using API.Core.ApiResponse;
using Application.Features.Users.Events.EmailConfirmed;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse<Unit>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        private readonly IMediator _mediator;

        public ConfirmEmailCommandHandler( UserManager<User> userManager, ILogService logService, IMediator mediator)
        {
            _userManager = userManager;
            _logService = logService;
            _mediator = mediator;
        }

        public async Task<ApiResponse<Unit>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.userMail);
            var confirmResult = await _userManager.ConfirmEmailAsync(user!, request.token);
            await _logService.CreateLogAsync($"User {user!.Email} confirmed email", LogType.Information, userId: user.Id);
            await _mediator.Publish(new EmailConfirmedEvent { Email = request.userMail });
            return ApiResponse<Unit>.Success(Unit.Value);

        }
    }
}
