using MediatR;

namespace Application.Features.Posts.Events.PostDeletedEvent
{
    public class PostDeletedEvent : INotification
    {
        public Guid postId { get; set; }
    }
}
