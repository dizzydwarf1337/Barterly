using MediatR;

namespace Application.Commands.Chat.PayPropose;

public class PayProposeCommand : IRequest<Unit>
{
    public Guid MessageId { get; set; }
}