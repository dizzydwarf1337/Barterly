using Application.Core.ApiResponse;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Public.Auth.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<Unit>>
{
    private readonly UserManager<User> _userManager;

    public LogoutCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.AuthorizeData.UserId.ToString()) ??
                   throw new EntityNotFoundException("User");
        await _userManager.RemoveAuthenticationTokenAsync(user, "App", "JWT");
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}