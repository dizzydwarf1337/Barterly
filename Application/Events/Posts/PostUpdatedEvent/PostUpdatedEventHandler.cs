using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Events.Posts.PostUpdatedEvent;

public class PostUpdatedEventHandler : INotificationHandler<PostUpdatedEvent>
{
    private readonly IMailService _mailService;
    private readonly INotificationCommandRepository _notificationCommandRepository;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public PostUpdatedEventHandler(IPostQueryRepository postQueryRepository,
        IUserQueryRepository userQueryRepository, IMailService mailService,
        INotificationCommandRepository notificationCommandRepository)
    {
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _mailService = mailService;
        _notificationCommandRepository = notificationCommandRepository;
    }

    public async Task Handle(PostUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUserAsync(notification.UserId, cancellationToken);
        var post = await _postQueryRepository.GetPostById(notification.PostId, cancellationToken);
        await _mailService.SendMail(user.Email!, "Post Updated",
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
        var usNotification = new Notification
        {
            Message = "Your post: " + post.Title + " has been updated", Title = "Post updated",
            UserId = post.OwnerId
        };
        await _notificationCommandRepository.CreateNotificationAsync(usNotification, cancellationToken);
    }
}