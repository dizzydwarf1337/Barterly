using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Unit>>
    {
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }


        async Task<ApiResponse<Unit>> IRequestHandler<CreateUserCommand, ApiResponse<Unit>>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _authService.Register(request.registerDto);
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
