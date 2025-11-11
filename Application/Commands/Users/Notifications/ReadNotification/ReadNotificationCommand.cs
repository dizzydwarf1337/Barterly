using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Users.Notifications.ReadNotification;

public class ReadNotificationCommand : UserRequest<Unit>
{
    public Guid NotificationId { get; set; }
}