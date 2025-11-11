using Domain.Interfaces.Commands.Chat;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.ReadMessage;

public class ReadMessageCommandHandler : IRequestHandler<ReadMessageCommand, Unit>
{
    private readonly IMessageQueryRepository _messageQueryRepository;
    private readonly IMessageCommandRepository _messageCommandRepository;

    public ReadMessageCommandHandler(IMessageQueryRepository messageQueryRepository,
        IMessageCommandRepository messageCommandRepository)
    {
        _messageQueryRepository = messageQueryRepository;
        _messageCommandRepository = messageCommandRepository;
    }
    
    public async Task<Unit> Handle(ReadMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageQueryRepository.GetMessages()
            .FirstOrDefaultAsync(x => x.Id == request.ReadMessage.MessageId, cancellationToken);

        if (message == null || message.ReadBy.HasValue && message.ReadBy == request.ReadMessage.ReadBy)
            return Unit.Value;

        message.IsRead = true;
        message.ReadAt = DateTime.UtcNow;
        message.ReadBy = request.ReadMessage.ReadBy;
        await _messageCommandRepository.UpdateMessage(message, cancellationToken);
        return Unit.Value;
    }
}