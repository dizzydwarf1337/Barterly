using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.General;
using Persistence.Database;

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
            var globalNotification = await _context.GlobalNotifications.FindAsync(id) ?? throw new EntityNotFoundException("Global notification");
            _context.GlobalNotifications.Remove(globalNotification);
            await _context.SaveChangesAsync();
        }
    }
}
