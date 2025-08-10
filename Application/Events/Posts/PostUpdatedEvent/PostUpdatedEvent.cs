using MediatR;

namespace Application.Events.Posts.PostUpdatedEvent;

public class PostUpdatedEvent : INotification
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}