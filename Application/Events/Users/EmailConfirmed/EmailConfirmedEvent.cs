using MediatR;

namespace Application.Events.Users.EmailConfirmed;

public class EmailConfirmedEvent : INotification
{
    public required string Email { get; set; }
}