using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostDeletedEvent
{
    public class PostDeletedEventHandler : INotificationHandler<PostDeletedEvent>
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;

        public PostDeletedEventHandler(IPostService postService, IUserService userService, IMailService mailService, INotificationService notificationService)
        {
            _postService = postService;
            _userService = userService;
            _mailService = mailService;
            _notificationService = notificationService;
        }

        public async Task Handle(PostDeletedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postService.GetPostByIdAdmin(notification.postId);
                var user = await _userService.GetUserById(Guid.Parse(post.OwnerId));
                await _mailService.SendMail(user.Email, "Your post has been deleted",
                     $@"
                     <body>
                      <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
                            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                                <h2 style=""color: #d9534f;"">Your post has been deleted</h2>
                                <p style='font-size: 16px; line-height: 1.6;'>
                                Your post <strong>{post.Title}</strong> has been removed from the platform.

                                </p>
                                 <div style='text-align: center; margin: 30px 0;'>
                                    <a href=""https://localhost:3000/support"" style= ' background-color: #d9534f;color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;'>
                                    Contact Support
                                    </a>
                                </div>
                                <p style='font-size: 14px; color: #777;'>If you believe this was a mistake, please contact our support team.</p>
                                <p style=""font-size: 14px; color: #777;"">
                                — The Barterly Team
                                </p>
                            </div>
                      </div>
                     </body>"
                    );
                var usNotification = new Notification { Message = $"Your post with name: {post.Title} has been deleted", Title = "Post deleted", UserId = Guid.Parse(user.Id) };
                await _notificationService.SendNotification(usNotification);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
