using API.Core.ApiResponse;
using Application.Features.Users.Events.UserCreated;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Unit>>
    {
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;

        public CreateUserCommandHandler(IAuthService authService,IMediator mediator, ILogService logService)
        {
            _authService = authService;
            _mediator = mediator;
            _logService = logService;
        }


        public async Task<ApiResponse<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _authService.Register(request.registerDto);
                await _mediator.Publish(new UserCreatedEvent { Email=request.registerDto.Email});
                return ApiResponse<Unit>.Success(Unit.Value,201);
            }
            catch(EntityNotFoundException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 404);
            }
            catch(InvalidOperationException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 400);
            }
            catch(AccessForbiddenException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 403);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure($"{ex.Message + ex.StackTrace}");
            }
        }
    }
}
