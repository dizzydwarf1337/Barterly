using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Events.UserCreated
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IMailService _mailService;
        private readonly IUserSettingsService _userSettingService;
        private readonly INotificationService _notificationService;
        private readonly IUserActivityService _userActivityService;

        public UserCreatedEventHandler(IMailService mailService, IUserSettingsService userSettingService, INotificationService notificationService, IUserActivityService userActivityService)
        {
            _mailService = mailService;
            _userSettingService = userSettingService;
            _notificationService = notificationService;
            _userActivityService = userActivityService;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _mailService.SendConfiramationMail(notification.Email);
                var userSettings = new UserSettings { UserId=notification.UserId };
                await _userSettingService.CreateUserSettings(userSettings);
                await _userActivityService.CreateUserActivity(notification.UserId);
                var userNotification = new Notification
                {
                    UserId = notification.UserId,
                    Message = "User has been registered",
                    Title = "Registration successful"
                };
                await _notificationService.SendNotification(userNotification);
            }
            catch (Exception ex) { }

        }
    }
}
