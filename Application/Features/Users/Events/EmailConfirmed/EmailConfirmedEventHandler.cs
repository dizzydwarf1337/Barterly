using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Events.EmailConfirmed
{
    public class EmailConfirmedEventHandler :INotificationHandler<EmailConfirmedEvent>
    {
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        public EmailConfirmedEventHandler(IMailService mailService, INotificationService notificationService, IUserService userService)
        {
            _mailService = mailService;
            _notificationService = notificationService;
            _userService = userService;
        }
        public async Task Handle(EmailConfirmedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetUserByEmail(notification.Email);
                await _mailService.SendMail(user.Email, "Email confirmed successfully",
                  $@"
                    <body>
                          <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
                            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                                <h2 style=""color: #2c3e50;"">Email confirmed successfully!</h2>
                               <p style='font-size: 16px; line-height: 1.6;'>
                                 Thank you for confirming your email address. Your account is now fully activated, and you can start using all features of our platform.
                               </p>
                               <div style='text-align: center; margin: 30px 0;'>
                                <a href=""https://localhost:3000"" style=""background-color: #4CAF50; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;"">
                                    Go to Website
                                </a>
                               </div>
                               <p style=""font-size: 14px; color: #777;"">
                                 — The Barterly Team
                               </p>
                             </div>
                           </div>
                    </body>
                  "); 
                var usNotification = new Notification
                {
                    Message = "Email confirmed successfully",
                    Title = "Email confirmed",
                    UserId = Guid.Parse(user.Id)
                };
                await _notificationService.SendNotification(usNotification);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
    
}
