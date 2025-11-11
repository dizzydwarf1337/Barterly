using Domain.Entities.Chat;
using Domain.Interfaces.Queries.Chat;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Chat;

public class MessageQueryRepository : BaseQueryRepository<BarterlyDbContext>, IMessageQueryRepository
{
    public MessageQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public IQueryable<Message> GetMessages()
    {
        return _context.Messages;
    }
}