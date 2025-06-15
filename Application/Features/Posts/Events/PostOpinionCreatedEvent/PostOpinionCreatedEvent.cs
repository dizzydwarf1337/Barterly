using Domain.Entities.Common;
using Domain.Entities.Posts;
using MediatR;


namespace Application.Features.Posts.Events.PostOpinionCreatedEvent
{
    public class PostOpinionCreatedEvent : INotification
    {
        public required PostOpinion PostOpinion { get; set; }

    }
}
