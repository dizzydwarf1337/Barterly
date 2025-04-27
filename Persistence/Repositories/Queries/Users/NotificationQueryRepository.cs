using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class NotificationQueryRepository : BaseQueryRepository<BarterlyDbContext>, INotificationQueryRepository
    {
        public NotificationQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id) ?? throw new EntityNotFoundException("Notification");
        }

        public async Task<ICollection<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<Notification>> GetPaginatedUserNotifications(Guid userId, int PageSize, int PageNumber)
        {
            return await _context.Notifications.Skip(PageSize * (PageNumber - 1)).Take(PageSize).Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
