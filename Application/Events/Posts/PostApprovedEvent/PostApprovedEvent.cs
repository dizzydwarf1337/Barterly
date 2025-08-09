using MediatR;

namespace Application.Events.Posts.PostApprovedEvent;

public class PostApprovedEvent : INotification
{
    public required Guid postId { get; set; }
}