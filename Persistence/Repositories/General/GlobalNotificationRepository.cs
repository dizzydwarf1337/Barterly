using Domain.Entities;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.General
{
    public class GlobalNotificationRepository : BaseRepository, IGlobalNotificationCommandRepository, IGlobalNotificationQueryRepository
    {
        public GlobalNotificationRepository(BarterlyDbContext context) : base(context) { }

        public async Task CreateGlobalNotificationAsync(GlobalNotification globalNotification)
        {
            await _context.GlobalNotifications.AddAsync(globalNotification);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteGlobalNotificationAsync(Guid id)
        {
            var globalNotification = await GetGlobalNotificationByIdAsync(id);
            _context.GlobalNotifications.Remove(globalNotification);
            await _context.SaveChangesAsync();
        }

        public async Task<GlobalNotification> GetGlobalNotificationByIdAsync(Guid id)
        {
            return await _context.GlobalNotifications.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Global Notification not found");
        }

        public async Task<ICollection<GlobalNotification>> GetGlobalNotificationsAsync()
        {
            return await _context.GlobalNotifications.ToListAsync();
        }
    }
}
