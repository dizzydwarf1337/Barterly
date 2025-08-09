using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Events.Posts.PostApprovedEvent;

public class PostApprovedEventHandler : INotificationHandler<PostApprovedEvent>
{
    private readonly IMailService _mailService;
    private readonly INotificationCommandRepository _notificationCommandRepository;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public PostApprovedEventHandler(IPostQueryRepository postQueryRepository,
        IUserQueryRepository userQueryRepository, IMailService mailService,
        INotificationCommandRepository notificationCommandRepository)
    {
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _mailService = mailService;
        _notificationCommandRepository = notificationCommandRepository;
    }

    public async Task Handle(PostApprovedEvent notification, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetPostById(notification.postId, cancellationToken);
        var user = await _userQueryRepository.GetUserAsync(post.OwnerId, cancellationToken);
        var usNotification = new Notification
        {
            Message = $"Congratulations your post ({post.Title}) has been approved",
            UserId = user.Id,
            Title = "Post approved!"
        };
        await _notificationCommandRepository.CreateNotificationAsync(usNotification, cancellationToken);
        await _mailService.SendMail(user.Email!, "Post has been approved!", $@"
            <body>
                <div style=""margin: 0; padding: 40px 20px; font-family: Arial, sans-serif; background-color: #f0f0f0;"">
                    <div style=""max-width: 600px; margin: auto; background: #fff; border-radius: 12px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); padding: 30px; text-align: left;"">
                        <h2 style=""color: #333; margin-top: 0;"">✅ Your post has been approved</h2>
                        <p style=""font-size: 16px; color: #555; line-height: 1.6;"">
                            We're happy to let you know that your post <strong>{post.Title}</strong> has been reviewed and approved by our moderators.<br><br>
                            You can now view it publicly on our platform.
                        </p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href=""https://localhost:3000/posts/{post.Id}"" style= ' background-color: #47b839;color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;'>
                                View Post
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
}