using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Events.Users.UserCreated;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IMailService _mailService;
    private readonly INotificationCommandRepository _notificationCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public UserCreatedEventHandler(IMailService mailService, IUserQueryRepository userQueryRepository,
        INotificationCommandRepository notificationCommandRepository)
    {
        _mailService = mailService;
        _userQueryRepository = userQueryRepository;
        _notificationCommandRepository = notificationCommandRepository;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUserByEmail(notification.Email, cancellationToken);
        await _mailService.SendConfiramationMail(notification.Email, cancellationToken);
        var userNotification = new Notification
        {
            UserId = user.Id,
            Message = "Account has been created successfully",
            Title = "Account created"
        };
        await _notificationCommandRepository.CreateNotificationAsync(userNotification, cancellationToken);
    }
}