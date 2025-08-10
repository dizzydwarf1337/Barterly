using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.General;

public class GlobalNotificationQueryRepository : BaseQueryRepository<BarterlyDbContext>,
    IGlobalNotificationQueryRepository
{
    public GlobalNotificationQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<GlobalNotification> GetGlobalNotificationByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.GlobalNotifications.FirstOrDefaultAsync(x => x.Id == id, token) ??
               throw new EntityNotFoundException("Global Notification");
    }

    public async Task<ICollection<GlobalNotification>> GetGlobalNotificationsAsync(CancellationToken token)
    {
        return await _context.GlobalNotifications.ToListAsync(token);
    }

    public IQueryable<GlobalNotification> GetPaginatedGlobalNotifications(int PageSize, int PageNumber)
    {
        return _context.GlobalNotifications.Skip(PageSize * (PageNumber - 1)).Take(PageSize);
    }
}