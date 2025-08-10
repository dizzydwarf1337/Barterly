using Domain.Entities.Users;
using MediatR;

namespace Application.Events.Opinions.UserOpinionCreated;

public class UserOpinionCreatedEvent : INotification
{
    public required UserOpinion opinion { get; set; }
}