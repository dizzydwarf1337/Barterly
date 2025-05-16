using MediatR;

namespace Application.Features.Posts.Events.PostCreatedEvent
{
    public class PostCreatedEvent : INotification
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

    }
}
