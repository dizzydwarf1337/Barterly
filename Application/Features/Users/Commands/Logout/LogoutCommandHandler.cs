using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<Unit>>
    {
        private readonly IAuthService _authService;
        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ApiResponse<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _authService.LogOut(request.token);

                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure("Error while logging out");
            }
        }
    }
}
