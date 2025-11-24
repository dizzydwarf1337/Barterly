using Domain.Entities.Chat;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Сhats.Messages.GetMessage;

public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, Message>
{
    private readonly IMessageQueryRepository _messageQueryRepository;
    
    public GetMessageQueryHandler(IMessageQueryRepository messageQueryRepository)
        => _messageQueryRepository = messageQueryRepository;
    
    public async Task<Message> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        var message =
            await _messageQueryRepository.GetMessages()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
            throw new EntityNotFoundException("Message not found");
        return message;
    }
}