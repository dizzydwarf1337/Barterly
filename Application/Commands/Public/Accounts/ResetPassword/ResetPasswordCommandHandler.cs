using Application.Core.ApiResponse;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Public.Accounts.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<Unit>>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new EntityNotFoundException("User");
        var res = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}