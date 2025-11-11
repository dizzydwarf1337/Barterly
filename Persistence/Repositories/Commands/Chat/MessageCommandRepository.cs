using Domain.Entities.Chat;
using Domain.Interfaces.Commands.Chat;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Chat;

public class MessageCommandRepository : BaseCommandRepository<BarterlyDbContext>, IMessageCommandRepository
{
    public MessageCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task SaveMessage(Message message, CancellationToken token)
    {
        await _context.Messages.AddAsync(message, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateMessage(Message message, CancellationToken token)
    {
        _context.Messages.Update(message);
        await _context.SaveChangesAsync(token);
    }
}