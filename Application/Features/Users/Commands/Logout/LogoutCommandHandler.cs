using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<Unit>>
    {
        private readonly UserManager<User> _userManager;

        public LogoutCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.userMail) ?? throw new EntityNotFoundException("User");
            await _userManager.RemoveAuthenticationTokenAsync(user, "App", "JWT");
            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
