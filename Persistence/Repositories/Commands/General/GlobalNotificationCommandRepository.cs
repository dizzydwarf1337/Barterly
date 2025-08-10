using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.General;
using Persistence.Database;

namespace Persistence.Repositories.Commands.General;

public class GlobalNotificationCommandRepository : BaseCommandRepository<BarterlyDbContext>,
    IGlobalNotificationCommandRepository
{
    public GlobalNotificationCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateGlobalNotificationAsync(GlobalNotification globalNotification, CancellationToken token)
    {
        await _context.GlobalNotifications.AddAsync(globalNotification, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteGlobalNotificationAsync(Guid id, CancellationToken token)
    {
        var globalNotification = await _context.GlobalNotifications.FindAsync(id, token) ??
                                 throw new EntityNotFoundException("Global notification");
        _context.GlobalNotifications.Remove(globalNotification);
        await _context.SaveChangesAsync(token);
    }
}