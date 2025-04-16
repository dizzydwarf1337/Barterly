using MediatR;

namespace Application.Features.Users.Events.UserCreated
{
    public class UserCreatedEvent : INotification
    {
        public UserCreatedEvent(string email, string firstName, string lastName, Guid userId)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
