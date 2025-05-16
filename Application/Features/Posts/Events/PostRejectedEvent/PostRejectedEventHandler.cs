using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostRejectedEvent
{
    public class PostRejectedEventHandler : INotificationHandler<PostRejectedEvent>
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;

        public PostRejectedEventHandler(IPostService postService, IUserService userService, IMailService mailService, INotificationService notificationService)
        {
            _postService = postService;
            _userService = userService;
            _mailService = mailService;
            _notificationService = notificationService;
        }

        public async Task Handle(PostRejectedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postService.GetPostByIdAdmin(Guid.Parse(notification.postId));
                var user = await _userService.GetUserById(Guid.Parse(post.OwnerId));
                var usNotification = new Notification
                {
                    UserId = Guid.Parse(post.OwnerId),
                    Title = "Post Rejected",
                    Message = $"Your post '{post.Title}' has been rejected.",
                };
                await _mailService.SendMail(user.Email, "Post has been rejected", $@"
                <body>
                    <div style=""margin: 0; padding: 40px 20px; font-family: Arial, sans-serif; background-color: #f0f0f0;"">
                        <div style=""max-width: 600px; margin: auto; background: #fff; border-radius: 12px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); padding: 30px; text-align: left;"">
                            <h2 style=""color: #d9534f; margin-top: 0;"">❌ Your post has been rejected</h2>
                            <p style=""font-size: 16px; color: #555; line-height: 1.6;"">
                                Unfortunately, your post <strong>{post.Title}</strong> has been reviewed by our moderators and did not meet our guidelines.<br><br>
                                Please review our content policy and consider updating your post.
                            </p>
                            <p style=""font-size: 16px; color: #555; line-height: 1.6;"">
                                <strong>Reason:</strong> {notification.reason}<br />
                            </p>
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href=""https://localhost:3000/support"" style=""background-color: #d9534f; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;"">
                                    Contact Support
                                </a>
                            </div>

                            <p style=""font-size: 14px; color: #888; margin-top: 30px;"">
                                If you have any questions, feel free to contact our support team.
                            </p>
                            <p style=""font-size: 14px; color: #888; margin-top: 10px;"">
                                — The Barterly Team
                            </p>
                        </div>
                    </div>
                </body>
");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
