using Domain.Entities.Common;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.General
{
    public class GlobalNotificationQueryRepository : BaseQueryRepository<BarterlyDbContext>, IGlobalNotificationQueryRepository
    {
        public GlobalNotificationQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<GlobalNotification> GetGlobalNotificationByIdAsync(Guid id)
        {
            return await _context.GlobalNotifications.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Global Notification not found");
        }

        public async Task<ICollection<GlobalNotification>> GetGlobalNotificationsAsync()
        {
            return await _context.GlobalNotifications.ToListAsync();
        }

        public async Task<ICollection<GlobalNotification>> GetPaginatedGlobalNotifications(int PageSize, int PageNumber)
        {
            return await _context.GlobalNotifications.Skip(PageSize * (PageNumber - 1)).Take(PageSize).ToListAsync();
        }
    }
}
