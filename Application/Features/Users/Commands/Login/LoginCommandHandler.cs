using API.Core.ApiResponse;
using Application.DTOs.User;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<UserDto>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ApiResponse<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return ApiResponse<UserDto>.Success(await _authService.Login(request.loginDto));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure(ex.Message + ex.StackTrace);
            }
        }
    }
}
