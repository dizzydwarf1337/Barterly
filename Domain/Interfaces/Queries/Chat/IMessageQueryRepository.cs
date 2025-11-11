using Domain.Entities.Chat;

namespace Domain.Interfaces.Queries.Chat;

public interface IMessageQueryRepository
{
    public IQueryable<Message> GetMessages();
}