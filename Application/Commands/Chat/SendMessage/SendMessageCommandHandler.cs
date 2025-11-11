using Domain.Entities.Chat;
using Domain.Interfaces.Commands.Chat;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Message>
{
    private readonly IChatQueryRepository  _chatQueryRepository;
    private readonly IChatCommandRepository _chatCommandRepository;
    private readonly IMessageCommandRepository _messageCommandRepository;

    public SendMessageCommandHandler(IChatQueryRepository chatQueryRepository,
        IChatCommandRepository chatCommandRepository, IMessageCommandRepository messageCommandRepository)
    {
        _chatQueryRepository = chatQueryRepository;
        _chatCommandRepository = chatCommandRepository;
        _messageCommandRepository = messageCommandRepository;
    }
    
    public async Task<Message> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            SenderId = request.Message.SenderId,
            ReceiverId = request.Message.ReceiverId,
            Content = request.Message.Content,
            ChatId = request.Message.ChatId ?? Guid.NewGuid(),
            Type = MessageType.Common
        };
        
        var chat = await _chatQueryRepository.GetChats()
            .FirstOrDefaultAsync(x=> x.Id == message.ChatId, cancellationToken);
        
        if (chat == null)
        {
            var chatToAdd = new Domain.Entities.Chat.Chat()
            {
                Id = message.ChatId,
                User1 = request.Message.SenderId,
                User2 = request.Message.ReceiverId
            };
            message.ChatId = chatToAdd.Id;  
            chatToAdd.Messages.Add(message);
            await _chatCommandRepository.CreateChat(chatToAdd, cancellationToken);
            
            return message;
        }
        
        await _messageCommandRepository.SaveMessage(message, cancellationToken);
        return message;
    }
}