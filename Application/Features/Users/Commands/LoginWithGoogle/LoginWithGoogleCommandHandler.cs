using API.Core.ApiResponse;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.LoginWithGoogle
{
    public class LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommand, ApiResponse<UserDto>>
    {
        private readonly IAuthService _authService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IMailService _mailService;
        private readonly IUserActivityService _userActivityService;

        public LoginWithGoogleCommandHandler(IAuthService authService, IUserSettingsService userSettingsService, IMailService mailService,IUserActivityService userActivityService)
        {
            _authService = authService;
            _userSettingsService = userSettingsService;
            _mailService = mailService;
            _userActivityService = userActivityService;
        }

        public async Task<ApiResponse<UserDto>> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await _authService.LoginWithGmail(request.googleLoginDto.token);
                var userId = Guid.Parse(userDto.Id);
                var userSettings = await _userSettingsService.GetUserSettingByUserIdAsync(userId).ContinueWith(x => x.IsFaulted ? null : x.Result);
                var userActivity = await _userActivityService.GetUserActivityByUserId(userId).ContinueWith(x=>x.IsFaulted ? null : x.Result);  

                if (userSettings == null)
                {
                    userSettings = new UserSettings { UserId = userId };
                    await _userSettingsService.CreateUserSettings(userSettings);


                    if (userActivity == null)
                    {
                        await _userActivityService.CreateUserActivity(userId);
                    }
                }
                await _mailService.SendMail(userDto.Email, "External login", @"
                <div>
                <h1>You have signed in with your gmail account</h1>
                </div>"
                );

                if (userActivity == null)
                {
                    await _userActivityService.CreateUserActivity(userId);
                }
                return ApiResponse<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure(ex.Message);
            }
        }
    }
}
