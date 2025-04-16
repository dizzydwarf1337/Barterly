using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class NotificationCommandRepository : BaseCommandRepository<BarterlyDbContext>, INotificationCommandRepository
    {
        public NotificationCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id) ?? throw new Exception("Notification not found");
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SetReadNotificationAsync(Guid id, bool IsRead)
        {
            var notification = await _context.Notifications.FindAsync(id) ?? throw new Exception("Notification not found");
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
