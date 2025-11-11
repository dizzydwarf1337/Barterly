using Application.Core.MediatR.Requests;
using Domain.Entities.Chat;

namespace Application.Queries.Users.Chat.GetChatMesages;

public class GetChatMessagesQuery : UserRequest<GetChatMessagesQuery.Result>
{
    public Guid ChatId { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    
    public class Result
    {
        public List<Message> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public record Message(
        Guid Id,
        Guid ChatId,
        string Content,
        MessageType Type,
        Guid SenderId,
        Guid ReceiverId,
        Guid? ReadBy,
        DateTime SentAt,
        DateTime? ReadAt,
        DateTime? AcceptedAt,
        decimal? Price,
        bool? IsAccepted,
        Guid? PostId);
}