using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostUpdatedEvent
{
    public class PostUpdatedEventHandler : INotificationHandler<PostUpdatedEvent>
    {
        private readonly IPostService _postService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public PostUpdatedEventHandler(IPostService postService, IMailService mailService, IUserService userService, INotificationService notificationService)
        {
            _postService = postService;
            _mailService = mailService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task Handle(PostUpdatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetUserById(Guid.Parse(notification.UserId));
                var post = await _postService.GetPostById(Guid.Parse(notification.PostId), Guid.Parse(notification.UserId));
                await _mailService.SendMail(user.Email, "Post Updated",
                      $@"
                       <body>
                        <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
                            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                               <h2 style=""color: #2c3e50; margin-top: 0;"">Your post has been updated!</h2>
                            <p  style='font-size: 16px; line-height: 1.6;'>
                             You've successfully updated the post: <strong>{post.Title}</strong>.<br/>
                             Click the button below to view it.
                            </p>
                            <div style='text-align: center; margin: 30px 0;'>
                                    <a href=""https://localhost:3000/posts/{post.Id}"" style='background-color: #4CAF50; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;'>
                                    View Post
                                    </a>
                            </div>
                           <p style=""font-size: 14px; color: #777; margin-top: 30px;"">
                             Our moderators will approve it in a couple of days. Thanks for your patience!
                           </p>
                           <p style=""font-size: 14px; color: #777; margin-top: 30px;"">
                             — The Barterly Team
                           </p>
                         </div>
                       </div>
                      </body>"
                      );
                var usNotification = new Notification { Message = "Your post: " + post.Title + " has been updated", Title = "Post updated", UserId = Guid.Parse(post.OwnerId) };
                await _notificationService.SendNotification(usNotification);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
