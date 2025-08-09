using Domain.Entities.Common;

namespace Domain.Interfaces.Queries.General;

public interface IGlobalNotificationQueryRepository
{
    Task<ICollection<GlobalNotification>> GetGlobalNotificationsAsync(CancellationToken token);
    Task<GlobalNotification> GetGlobalNotificationByIdAsync(Guid id, CancellationToken token);
}