using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostCreatedEvent
{
    public class PostCreatedEventHanlder : INotificationHandler<PostCreatedEvent>
    {
        private readonly IMailService _mailService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public PostCreatedEventHanlder(IMailService mailService, IPostService postService, IUserService userService, INotificationService notificationService)
        {
            _mailService = mailService;
            _postService = postService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetUserById(notification.UserId);
                var post = await _postService.GetPostById(notification.PostId,notification.UserId);
                if (user != null && post != null)
                {
                    await _mailService.SendMail(user.Email, "Post created succesfully 🎉",
                         $@"
                           </body>
                            <div style=""margin:0; padding:40px 20px; font-family: Arial, sans-serif; background-color: #f0f0f0;"">
                              <div style=""max-width: 600px; margin: auto; background: #fff; border-radius: 12px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); padding: 30px; text-align: left;"""">
                                <h2 style=""color: #333;"">📝 New Post Created</h2>
                                <p style=""font-size: 16px;   line-height: 1.6;""><strong>Title:</strong> {post.Title}</p>
                                <p style=""font-size: 16px;   line-height: 1.6;""><strong>Location:</strong> {post.City}, {post.Region}</p>
                                <p style=""font-size: 16px;  line-height: 1.6;""><strong>Short Description:</strong> {post.ShortDescription}</p>
                                <div style='text-align: center; margin: 30px 0;'>
                                    <a href=""https://localhost:3000/posts/{post.Id}"" style=""background-color: #008b8b;color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;"">
                                    View Post
                                    </a>
                                </div>
                                <p style=""font-size: 14px; color: #777; "">
                                  Now please wait for our moderators to confirm your post. Once it’s approved, you will receive an email.
                                </p>
                                <p style=""font-size: 14px; color: #777; "">
                                  — The Barterly Team
                                </p>
                              </div>
                            </div>
                           </body>"
                        );
                }
                var usNotification = new Notification { 
                    Message = $"Post:{post.Title} created. Awaiting review. Email will be sent after approval.",
                    Title = "Post created", 
                    UserId = Guid.Parse(post.OwnerId)
                };
                await _notificationService.SendNotification(usNotification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message} + {ex.StackTrace}");
                // Handle the error (e.g., log it)

            }
        }
    }
}
