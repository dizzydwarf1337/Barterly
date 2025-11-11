using Application.Core.MediatR.Requests;
using Application.Queries.Users.Chat.GetChatMesages;

namespace Application.Queries.Users.Chat.GetMyChats;

public class GetMyChatsQuery : UserRequest<ICollection<Chat>>;

public record Chat (Guid Id, DateTime CreatedAt, User User1, User User2, List<GetChatMessagesQuery.Message>? Messages);

public record User (Guid UserId, string FirstName, string LastName, string? ImagePath);