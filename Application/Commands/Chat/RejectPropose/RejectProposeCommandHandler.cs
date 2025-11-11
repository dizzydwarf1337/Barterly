using Domain.Interfaces.Commands.Chat;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.RejectPropose;

public class RejectProposeCommandHandler : IRequestHandler<RejectProposeCommand, Unit>
{
    private readonly IMessageQueryRepository _messageQueryRepository;
    private readonly IMessageCommandRepository _messageCommandRepository;

    public RejectProposeCommandHandler(IMessageQueryRepository messageQueryRepository,
        IMessageCommandRepository messageCommandRepository)
    {
        _messageQueryRepository = messageQueryRepository;
        _messageCommandRepository = messageCommandRepository;
    }
    
    public async Task<Unit> Handle(RejectProposeCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageQueryRepository.GetMessages()
            .FirstOrDefaultAsync(x => x.Id == request.Reject.MessageId, cancellationToken);
        if(message == null)
            return Unit.Value;

        message.IsAccepted = false;
        await _messageCommandRepository.UpdateMessage(message, cancellationToken);
        return Unit.Value;
    }
}