using Domain.Entities;
using Domain.Interfaces.Commands.General;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.General
{
    public class GlobalNotificationCommandRepository : BaseCommandRepository<BarterlyDbContext>, IGlobalNotificationCommandRepository
    {
        public GlobalNotificationCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateGlobalNotificationAsync(GlobalNotification globalNotification)
        {
            await _context.GlobalNotifications.AddAsync(globalNotification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGlobalNotificationAsync(Guid id)
        {
            var globalNotification = await _context.GlobalNotifications.FindAsync(id) ?? throw new Exception("Notification not found");
            _context.GlobalNotifications.Remove(globalNotification);
            await _context.SaveChangesAsync();
        }
    }
}
