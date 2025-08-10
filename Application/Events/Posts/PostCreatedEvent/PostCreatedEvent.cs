using MediatR;

namespace Application.Events.Posts.PostCreatedEvent;

public class PostCreatedEvent : INotification
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}