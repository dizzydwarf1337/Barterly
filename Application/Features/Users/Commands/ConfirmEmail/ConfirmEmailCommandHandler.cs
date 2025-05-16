using API.Core.ApiResponse;
using Application.Features.Users.Events.EmailConfirmed;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using MediatR;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse<Unit>>
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public ConfirmEmailCommandHandler(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        public async Task<ApiResponse<Unit>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.ConfirmEmail(request.userMail, request.token);
                await _mediator.Publish(new EmailConfirmedEvent { Email = request.userMail });
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 404);
            }
            catch (AccessForbiddenException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 403);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message + ex.StackTrace);
            }
        }
    }
}
