using Domain.Entities.Chat;
using MediatR;

namespace Application.Commands.Chat.PayPropose;

public class PayProposeCommand : IRequest<Message>
{
    public Guid MessageId { get; set; }
}