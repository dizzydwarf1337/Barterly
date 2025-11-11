using Domain.Interfaces.Commands.Chat;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Chat;

public class ChatCommandRepository : BaseCommandRepository<BarterlyDbContext>, IChatCommandRepository
{
    public ChatCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<Guid> CreateChat(Domain.Entities.Chat.Chat chat, CancellationToken token)
    {
        await _context.Chats.AddAsync(chat, token);
        await _context.SaveChangesAsync(token);
        return chat.Id;
    }
}