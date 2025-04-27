using API.Core.ApiResponse;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
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
            catch(InvalidDataProvidedException ex)
            {
                return ApiResponse<UserDto>.Failure(ex.Message);
            }
            catch(AccessForbiddenException ex)
            {
                return ApiResponse<UserDto>.Failure(ex.Message, 403);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure(ex.Message);
            }
        }
    }
}
