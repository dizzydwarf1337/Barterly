using MediatR;

namespace Application.Features.Users.Events.UserCreated
{
    public class UserCreatedEvent : INotification
    {
        public string Email { get; set; }
    }
}
