using MediatR;

namespace Application.Features.Users.Events.UserCreated
{
    public class UserCreatedEvent : INotification
    {
        public required string Email { get; set; }
    }
}
