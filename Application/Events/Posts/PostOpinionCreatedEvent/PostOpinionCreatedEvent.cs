using Domain.Entities.Posts;
using MediatR;

namespace Application.Events.Posts.PostOpinionCreatedEvent;

public class PostOpinionCreatedEvent : INotification
{
    public required PostOpinion PostOpinion { get; set; }
}