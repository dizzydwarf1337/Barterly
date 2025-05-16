using MediatR;

namespace Application.Features.Posts.Events.PostUpdatedEvent
{
    public class PostUpdatedEvent : INotification
    {
        public string PostId { get; set; }
        public string UserId { get; set; } 
    }
}
