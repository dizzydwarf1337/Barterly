using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Features.Posts.Events.PostDeletedEvent
{
    public class PostDeletedEventHandler : INotificationHandler<PostDeletedEvent>
    {
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IMailService _mailService;
        private readonly INotificationCommandRepository _notificationCommandRepository;

        public PostDeletedEventHandler(IPostQueryRepository postQueryRepository, IUserQueryRepository userQueryRepository, IMailService mailService, INotificationCommandRepository notificationCommandRepository)
        {
            _postQueryRepository = postQueryRepository;
            _userQueryRepository = userQueryRepository;
            _mailService = mailService;
            _notificationCommandRepository = notificationCommandRepository;
        }

        public async Task Handle(PostDeletedEvent notification, CancellationToken cancellationToken)
        {

            var post = await _postQueryRepository.GetPostById(notification.postId,null,"Admin");
            var user = await _userQueryRepository.GetUserAsync(post.OwnerId);
            await _mailService.SendMail(user.Email!, "Your post has been deleted",
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
            var usNotification = new Notification { Message = $"Your post with name: {post.Title} has been deleted", Title = "Post deleted", UserId = user.Id};
            await _notificationCommandRepository.CreateNotificationAsync(usNotification);

        }
    }
}
