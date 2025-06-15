using Domain.Entities.Common;
using Domain.Entities.Users;
using MediatR;

namespace Application.Features.Users.Events.UserOpinionCreated
{
    public class UserOpinionCreatedEvent : INotification
    {
        public required UserOpinion opinion { get; set; }
    }
}
