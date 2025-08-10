using MediatR;

namespace Application.Events.Posts.PostVisitedEvent;

public class PostVisitedEvent : INotification
{
    public Guid PostId { get; set; }
    public string? UserId { get; set; }
}