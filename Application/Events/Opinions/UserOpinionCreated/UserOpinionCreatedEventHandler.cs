using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Events.Opinions.UserOpinionCreated;

public class UserOpinionCreatedEventHandler : INotificationHandler<UserOpinionCreatedEvent>
{
    private readonly IMailService _mailService;
    private readonly INotificationCommandRepository _notificationCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public UserOpinionCreatedEventHandler(IMailService mailService,
        INotificationCommandRepository notificationCommandRepository, IUserQueryRepository userQueryRepository)
    {
        _mailService = mailService;
        _notificationCommandRepository = notificationCommandRepository;
        _userQueryRepository = userQueryRepository;
    }

    public async Task Handle(UserOpinionCreatedEvent notification, CancellationToken cancellationToken)
    {
        var usNotification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = notification.opinion.UserId,
            Title = "You got new opinion",
            Message = $"Your got new opinion with rate {notification.opinion.Rate}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        await _notificationCommandRepository.CreateNotificationAsync(usNotification, cancellationToken);
        var user = await _userQueryRepository.GetUserAsync(notification.opinion.AuthorId, cancellationToken);
        var opinionReceiver =
            await _userQueryRepository.GetUserAsync(notification.opinion.UserId, cancellationToken);
        await _mailService.SendMail(opinionReceiver.Email!, "New opinion received",
            $@"
                    <body>
                    <div style=""margin:0; padding:40px 20px; font-family: Arial, sans-serif; background-color: #f0f0f0;"">
                        <div style=""max-width: 600px; margin: auto; background: #fff; border-radius: 12px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); padding: 30px; text-align: left;"">
                            <h2 style=""color: #333;"">🌟 You've Received a New Rating</h2>
                            <p style=""font-size: 16px; line-height: 1.6;"">
                                Someone has left feedback about their interaction with you.
                            </p>

                            <div style=""margin: 30px 0; padding: 20px; background-color: #f9f9f9; border-radius: 10px; border: 1px solid #e0e0e0;"">
                                <div style=""display: flex; align-items: center;"">
                                    <img src=""{user.ProfilePicturePath ?? "https://via.placeholder.com/50"}"" alt=""Profile Picture"" style=""width: 50px; height: 50px; border-radius: 50%; object-fit: cover; margin-right: 15px;"">
                                    <div>
                                        <p style=""margin: 0; font-size: 16px;""><strong>{user.FirstName} {user.LastName}</strong></p>
                                        <div style=""color: #f5c518; font-size: 16px;"">
                                            {string.Concat(Enumerable.Repeat("★", notification.opinion.Rate ?? 0))}{string.Concat(Enumerable.Repeat("☆", 5 - (notification.opinion.Rate ?? 0)))}
                                        </div>
                                    </div>
                                </div>
                                <p style=""margin-top: 15px; font-size: 15px; line-height: 1.5; color: #333;"">
                                    {notification.opinion.Content}
                                </p>
                            </div>

                            <div style='text-align: center; margin: 30px 0;' >
                                <a href=""https://localhost:3000/profile/{user.Id}"" style=""background-color: #008b8b; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;"">
                                    View Your Profile
                                </a>
                            </div>
                            <p style=""font-size: 14px; color: #777;"">
                                You received this message because another user rated you. Keep up the good work and stay active!
                            </p>
                            <p style=""font-size: 14px; color: #777;"">
                                — The Barterly Team
                            </p>
                        </div>
                    </div>
                    </body>"
        );
    }
}