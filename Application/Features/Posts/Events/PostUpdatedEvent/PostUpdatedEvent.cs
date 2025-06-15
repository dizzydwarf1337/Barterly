using MediatR;

namespace Application.Features.Posts.Events.PostUpdatedEvent
{
    public class PostUpdatedEvent : INotification
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; } 
    }
}
