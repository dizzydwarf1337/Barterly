using Domain.Interfaces.Commands.Chat;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.AcceptPropose;

public class AcceptProposeCommandHandler : IRequestHandler<AcceptProposeCommand, Unit>
{
    private readonly IMessageQueryRepository _messageQueryRepository;
    private readonly IMessageCommandRepository _messageCommandRepository;

    public AcceptProposeCommandHandler(IMessageQueryRepository messageQueryRepository,
        IMessageCommandRepository messageCommandRepository)
    {
        _messageQueryRepository = messageQueryRepository;
        _messageCommandRepository = messageCommandRepository;
    }
    
    public async Task<Unit> Handle(AcceptProposeCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageQueryRepository.GetMessages()
            .FirstOrDefaultAsync(x => x.Id == request.AcceptPropose.MessageId, cancellationToken);
        if(message == null)
            return Unit.Value;
        
        message.IsAccepted = true;
        message.AcceptedAt = DateTime.UtcNow;
        await _messageCommandRepository.UpdateMessage(message, cancellationToken);
        return Unit.Value;
    }
}