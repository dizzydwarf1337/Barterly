using Domain.Entities.Chat;
using Domain.Interfaces.Commands.Chat;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.SendPropose;

// ✅ ИСПРАВЛЕНО: Возвращаем Message вместо Unit
public class SendProposeCommandHandler : IRequestHandler<SendProposeCommand, Message>
{
    private readonly IChatQueryRepository  _chatQueryRepository;
    private readonly IChatCommandRepository _chatCommandRepository;
    private readonly IMessageCommandRepository _messageCommandRepository;

    public SendProposeCommandHandler(IChatQueryRepository chatQueryRepository,
        IChatCommandRepository chatCommandRepository, IMessageCommandRepository messageCommandRepository)
    {
        _chatQueryRepository = chatQueryRepository;
        _chatCommandRepository = chatCommandRepository;
        _messageCommandRepository = messageCommandRepository;
    }
    
    public async Task<Message> Handle(SendProposeCommand request, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            SenderId = request.Message.SenderId,
            ReceiverId = request.Message.ReceiverId,
            Content = request.Message.Content,
            Type = MessageType.Proposal,
            Price = request.Message.Price,
            PostId = request.Message.PostId
        };
        
        var chat = await _chatQueryRepository.GetChats()
            .FirstOrDefaultAsync(x=> (x.User1 == request.Message.SenderId && x.User2 == request.Message.ReceiverId)
                                     || (x.User1 == request.Message.ReceiverId && x.User2 == request.Message.SenderId), cancellationToken);
        
        if (chat == null)
        {
            var chatToAdd = new Domain.Entities.Chat.Chat()
            {
                Id = Guid.NewGuid(), 
                User1 = request.Message.SenderId,
                User2 = request.Message.ReceiverId
            };
            message.ChatId = chatToAdd.Id; 
            chatToAdd.Messages.Add(message);
            await _chatCommandRepository.CreateChat(chatToAdd, cancellationToken);
            
            return message;
        }
        
        message.ChatId = chat.Id;
        await _messageCommandRepository.SaveMessage(message, cancellationToken);
        
        return message;
    }
}