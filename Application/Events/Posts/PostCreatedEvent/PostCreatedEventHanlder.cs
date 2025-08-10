using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Events.Posts.PostCreatedEvent;

public class PostCreatedEventHanlder : INotificationHandler<PostCreatedEvent>
{
    private readonly IMailService _mailService;
    private readonly INotificationCommandRepository _notificationCommandRepository;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public PostCreatedEventHanlder(IPostQueryRepository postQueryRepository,
        IUserQueryRepository userQueryRepository, IMailService mailService,
        INotificationCommandRepository notificationCommandRepository)
    {
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _mailService = mailService;
        _notificationCommandRepository = notificationCommandRepository;
    }

    public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUserAsync(notification.UserId, cancellationToken);
        var post = await _postQueryRepository.GetPostById(notification.PostId, cancellationToken);
        if (user != null && post != null)
            await _mailService.SendMail(user.Email!, "Post created succesfully 🎉",
                $@"
                        <body>
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

        var usNotification = new Notification
        {
            Message = $"Post:{post!.Title} created. Awaiting review. Email will be sent after approval.",
            Title = "Post created",
            UserId = post.OwnerId
        };
        await _notificationCommandRepository.CreateNotificationAsync(usNotification, cancellationToken);
    }
}