using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class NotificationQueryRepository : BaseQueryRepository<BarterlyDbContext>, INotificationQueryRepository
{
    public NotificationQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public IQueryable<Notification> GetAllNotificationsAsync()
    {
        return _context.Notifications;
    }

    public async Task<Notification> GetNotificationAsync(Guid id, CancellationToken token)
    {
        return await _context.Notifications.FindAsync(id, token) ??
               throw new EntityNotFoundException("Notification");
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.Notifications.Where(x => x.UserId == userId).OrderByDescending(x=> x.CreatedAt ).ToListAsync(token);
    }

    public async Task<ICollection<Notification>> GetPaginatedUserNotifications(Guid userId, int PageSize,
        int PageNumber, CancellationToken token)
    {
        return await _context.Notifications.Skip(PageSize * (PageNumber - 1)).Take(PageSize)
            .Where(x => x.UserId == userId).ToListAsync(token);
    }
}