using MediatR;

namespace Application.Events.Users.UserCreated;

public class UserCreatedEvent : INotification
{
    public required string Email { get; set; }
}