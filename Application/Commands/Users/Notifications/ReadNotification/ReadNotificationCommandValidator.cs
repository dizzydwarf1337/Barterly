using FluentValidation;

namespace Application.Commands.Users.Notifications.ReadNotification;

public class ReadNotificationCommandValidator : AbstractValidator<ReadNotificationCommand>
{
    public ReadNotificationCommandValidator()
    {
        RuleFor(x => x.NotificationId).NotEmpty();
    }
}