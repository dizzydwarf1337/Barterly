using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;

namespace Application.Features.Users.Events.UserCreated
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public UserCreatedEventHandler(IMailService mailService, IUserSettingsService userSettingService, INotificationService notificationService, IUserActivityService userActivityService, IUserService userService)
        {
            _mailService = mailService;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetUserByEmail(notification.Email);
                await _mailService.SendConfiramationMail(notification.Email);
                var userNotification = new Notification
                {
                    UserId = Guid.Parse(user.Id) ,
                    Message = "Account has been created successfully",
                    Title = "Account created"
                };
                await _notificationService.SendNotification(userNotification);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

        }
    }
}
