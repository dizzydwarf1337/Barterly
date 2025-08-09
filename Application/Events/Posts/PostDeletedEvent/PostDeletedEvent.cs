using MediatR;

namespace Application.Events.Posts.PostDeletedEvent;

public class PostDeletedEvent : INotification
{
    public Guid postId { get; set; }
}