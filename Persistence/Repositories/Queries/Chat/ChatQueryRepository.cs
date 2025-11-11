using Domain.Interfaces.Queries.Chat;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Chat;

public class ChatQueryRepository : BaseQueryRepository<BarterlyDbContext>, IChatQueryRepository
{
    public ChatQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public IQueryable<Domain.Entities.Chat.Chat> GetChats()
    {
        return _context.Chats;
    }
}