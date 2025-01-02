using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class NotificationRepository : BaseRepository, INotificationCommandRepository, INotificationQueryRepository
    {
        public NotificationRepository(BarterlyDbContext context) : base(context) { }

        public async Task CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Guid id)
        {
            var notification = await GetNotificationAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id) ?? throw new Exception("Notification not found");
        }

        public async Task<ICollection<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task SetReadNotificationAsync(Guid id, bool IsRead)
        {
            var notification = await GetNotificationAsync(id);
            notification.IsRead = IsRead;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}
