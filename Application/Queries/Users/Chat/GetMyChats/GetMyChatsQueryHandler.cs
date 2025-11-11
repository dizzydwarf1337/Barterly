using Application.Core.ApiResponse;
using Application.Queries.Users.Chat.GetChatMesages;
using Domain.Interfaces.Queries.Chat;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Chat.GetMyChats;

public class GetMyChatsQueryHandler : IRequestHandler<GetMyChatsQuery, ApiResponse<ICollection<Chat>>>
{
    private readonly IChatQueryRepository _chatQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public GetMyChatsQueryHandler(IChatQueryRepository chatQueryRepository, IUserQueryRepository userQueryRepository)
    {
        _chatQueryRepository = chatQueryRepository;
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<ApiResponse<ICollection<Chat>>> Handle(GetMyChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _chatQueryRepository.GetChats()
            .Include(x => x.Messages)
            .Where(x =>
                x.User1 == request.AuthorizeData.UserId ||
                x.User2 == request.AuthorizeData.UserId)
            .ToListAsync(cancellationToken);

        var userIds = chats.SelectMany(c => new[] { c.User1, c.User2 }).Distinct();
        var users = await _userQueryRepository.GetUsers()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        var userDict = users.ToDictionary(u => u.Id);
        var result = chats.Select(c => new Chat(
            c.Id,
            c.CreatedAt,
            new User(userDict[c.User1].Id, userDict[c.User1].FirstName, userDict[c.User1].LastName, userDict[c.User1].ProfilePicturePath),
            new User(userDict[c.User2].Id, userDict[c.User2].FirstName, userDict[c.User2].LastName, userDict[c.User2].ProfilePicturePath),
            c.Messages.OrderByDescending(m => m.SentAt).Take(1).Select(x => new GetChatMessagesQuery.Message(x.Id, x.ChatId, x.Content, x.Type, x.SenderId, x.ReceiverId, x.ReadBy, x.SentAt, x.ReadAt, x.AcceptedAt, x.Price, x.IsAccepted, x.PostId)).ToList()
        )).ToList();
        return ApiResponse<ICollection<Chat>>.Success(result);
    }
}