using API.Core.ApiResponse;
using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService, IMapper mapper, ILogService logService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<ApiResponse<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.loginDto.Email) ?? throw new EntityNotFoundException("User");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.loginDto.Password);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isPasswordCorrect)
            {
                return ApiResponse<UserDto>.Failure("Wrong password");
            }
            if (!isEmailConfirmed)
            {
                return ApiResponse<UserDto>.Failure("Confirm email first");
            }
            user.LastSeen = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            await _logService.CreateLogAsync("User logged in", LogType.Information, userId: user.Id);
            var token = await _tokenService.GetLoginToken(user.Id);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.token = token;
            var roles = await _userManager.GetRolesAsync(user);
            userDto.Role = roles.Contains("Admin") ? "Admin" :
                           roles.Contains("Moderator") ? "Moderator" :
                           "User";
            return ApiResponse<UserDto>.Success(userDto);
        }
    }
}
