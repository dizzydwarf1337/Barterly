using Application.Core.ApiResponse;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Public.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginCommand.Result>>
{
    private readonly ILogService _logService;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService, IMapper mapper,
        ILogService logService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _logService = logService;
    }

    public async Task<ApiResponse<LoginCommand.Result>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new EntityNotFoundException("User");

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isPasswordCorrect) return ApiResponse<LoginCommand.Result>.Failure("Wrong password");

        if (!isEmailConfirmed) return ApiResponse<LoginCommand.Result>.Failure("Confirm email first");

        user.LastSeen = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        await _logService.CreateLogAsync("User logged in", cancellationToken, LogType.Information, userId: user.Id);
        var token = await _tokenService.GetLoginToken(user.Id, cancellationToken);
        var roles = await _userManager.GetRolesAsync(user);
        return ApiResponse<LoginCommand.Result>.Success(new LoginCommand.Result
        {
            Email = user.Email,
            FirstName = user.FirstName,
            Id = user.Id.ToString(),
            LastName = user.LastName,
            ProfilePicturePath = user.ProfilePicturePath,
            Role = roles.Contains("Admin") ? "Admin" :
                roles.Contains("Moderator") ? "Moderator" : "User",
            token = token
        });
    }
}