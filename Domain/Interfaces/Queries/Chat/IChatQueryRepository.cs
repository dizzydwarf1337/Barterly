namespace Domain.Interfaces.Queries.Chat;

public interface IChatQueryRepository
{
    public IQueryable<Entities.Chat.Chat> GetChats();
}