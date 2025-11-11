using Domain.Interfaces.Commands.Chat;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.PayPropose;

public class PayProposeCommandHandler : IRequestHandler<PayProposeCommand, Unit>
{
    private readonly IMessageQueryRepository _messageQueryRepository;
    private readonly IMessageCommandRepository _messageCommandRepository;

    public PayProposeCommandHandler(IMessageQueryRepository messageQueryRepository,
        IMessageCommandRepository messageCommandRepository)
    {
        _messageQueryRepository = messageQueryRepository;
        _messageCommandRepository = messageCommandRepository;
    }
    
    public async Task<Unit> Handle(PayProposeCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageQueryRepository.GetMessages()
            .FirstOrDefaultAsync(x => x.Id == request.MessageId, cancellationToken);
        
        if(message == null)
            return Unit.Value;
        
        message.IsPaid = true;
        await _messageCommandRepository.UpdateMessage(message, cancellationToken);
        
        return Unit.Value;
    }
}