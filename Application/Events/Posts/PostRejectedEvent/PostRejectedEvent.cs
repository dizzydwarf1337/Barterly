using MediatR;

namespace Application.Events.Posts.PostRejectedEvent;

public class PostRejectedEvent : INotification
{
    public required Guid postId { get; set; }
    public required string reason { get; set; }
}