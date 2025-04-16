using Domain.Entities.Common;

namespace Domain.Interfaces.Queries.General
{
    public interface IGlobalNotificationQueryRepository
    {
        Task<ICollection<GlobalNotification>> GetGlobalNotificationsAsync();
        Task<GlobalNotification> GetGlobalNotificationByIdAsync(Guid id);
        Task<ICollection<GlobalNotification>> GetPaginatedGlobalNotifications(int PageSize, int PageNumber);
    }
}
