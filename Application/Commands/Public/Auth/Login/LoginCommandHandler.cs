using Application.Core.ApiResponse;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Public.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginCommand.Result>>
{
    private readonly ILogService _logService;
    private readonly ITokenService _tokenService;
    private readonly IUserSettingQueryRepository _userSettingQueryRepository;
    private readonly UserManager<User> _userManager;

    public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService, ILogService logService, IUserSettingQueryRepository userSettingQueryRepository)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userSettingQueryRepository = userSettingQueryRepository;
        _logService = logService;
    }

    public async Task<ApiResponse<LoginCommand.Result>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new EntityNotFoundException("User");
        var settings = await _userSettingQueryRepository.GetUserSettingByUserIdAsync(user.Id,cancellationToken);
        if(settings.IsDeleted)
            return ApiResponse<LoginCommand.Result>.Failure("User deleted");
        var roles =  (await _userManager.GetRolesAsync(user)).ToList();
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isPasswordCorrect) return ApiResponse<LoginCommand.Result>.Failure("Wrong password");

        if (!isEmailConfirmed) return ApiResponse<LoginCommand.Result>.Failure("Confirm email first");

        user.LastSeen = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        await _logService.CreateLogAsync("User logged in", cancellationToken, LogType.Information, userId: user.Id);
        var token = await _tokenService.GetLoginToken(user.Id, cancellationToken);
        return ApiResponse<LoginCommand.Result>.Success(new LoginCommand.Result()
        {
            Token = token,
            Roles = roles,
        });
    }
}