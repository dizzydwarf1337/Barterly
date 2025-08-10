using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Events.Posts.PostRejectedEvent;

public class PostRejectedEventHandler : INotificationHandler<PostRejectedEvent>
{
    private readonly IMailService _mailService;
    private readonly INotificationCommandRepository _notificationCommandRepository;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public PostRejectedEventHandler(IPostQueryRepository postQueryRepository,
        IUserQueryRepository userQueryRepository, IMailService mailService,
        INotificationCommandRepository notificationCommandRepository)
    {
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _mailService = mailService;
        _notificationCommandRepository = notificationCommandRepository;
    }

    public async Task Handle(PostRejectedEvent notification, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetPostById(notification.postId, cancellationToken);
        var user = await _userQueryRepository.GetUserAsync(post.OwnerId, cancellationToken);
        var usNotification = new Notification
        {
            UserId = post.OwnerId,
            Title = "Post Rejected",
            Message = $"Your post '{post.Title}' has been rejected."
        };
        await _mailService.SendMail(user.Email!, "Post has been rejected", $@"
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
        await _notificationCommandRepository.CreateNotificationAsync(usNotification, cancellationToken);
    }
}