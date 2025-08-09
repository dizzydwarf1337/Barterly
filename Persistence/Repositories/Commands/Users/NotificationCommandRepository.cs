using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users;

public class NotificationCommandRepository : BaseCommandRepository<BarterlyDbContext>,
    INotificationCommandRepository
{
    public NotificationCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateNotificationAsync(Notification notification, CancellationToken token)
    {
        await _context.Notifications.AddAsync(notification, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteNotificationAsync(Guid id, CancellationToken token)
    {
        var notification = await _context.Notifications.FindAsync(id, token) ??
                           throw new EntityNotFoundException("Notification");
        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync(token);
    }

    public async Task SetReadNotificationAsync(Guid id, bool IsRead, CancellationToken token)
    {
        var notification = await _context.Notifications.FindAsync(id, token) ??
                           throw new EntityNotFoundException("Notification");
        notification.IsRead = IsRead;
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateNotificationAsync(Notification notification, CancellationToken token)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync(token);
    }
}