using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Chat.GetChatMesages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, ApiResponse<GetChatMessagesQuery.Result>>
{
    private readonly IChatQueryRepository _chatQueryRepository;
    private readonly IMessageQueryRepository _messageQueryRepository;

    public GetChatMessagesQueryHandler(IChatQueryRepository chatQueryRepository,
        IMessageQueryRepository messageQueryRepository)
    {
        _chatQueryRepository = chatQueryRepository;
        _messageQueryRepository = messageQueryRepository;
    }
        
    
    public async Task<ApiResponse<GetChatMessagesQuery.Result>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var chat = await _chatQueryRepository.GetChats().FirstOrDefaultAsync(x =>
            x.Id == request.ChatId &&
            (x.User1 == request.AuthorizeData.UserId || x.User2 == request.AuthorizeData.UserId), cancellationToken);
        
        if(chat is null)
            return ApiResponse<GetChatMessagesQuery.Result>.Failure("Chat not found", 404);
        
        var messages = _messageQueryRepository.GetMessages().Where(x => x.ChatId == chat.Id);
        var messagesCount = await messages.CountAsync(cancellationToken);
        return ApiResponse<GetChatMessagesQuery.Result>.Success(new GetChatMessagesQuery.Result()
        {
            Items = await messages
                .OrderBy(x => x.SentAt)
                .Skip(request.Page -1)
                .Take(request.PageSize)
                .Select(x => 
                    new GetChatMessagesQuery.Message(
                        x.Id,
                        x.ChatId,
                        x.Content,
                        x.Type,
                        x.SenderId,
                        x.ReceiverId,
                        x.ReadBy,
                        x.SentAt,
                        x.ReadAt,
                        x.AcceptedAt,
                        x.Price,
                        x.IsAccepted,
                        x.PostId
                    )
                )
                .ToListAsync(cancellationToken),
            TotalCount = messagesCount,
            TotalPages = messagesCount / request.PageSize
        });
    }
}