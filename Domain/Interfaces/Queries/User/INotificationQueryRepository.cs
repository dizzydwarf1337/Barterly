using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface INotificationQueryRepository
{
    Task<Notification> GetNotificationAsync(Guid id, CancellationToken token);
    IQueryable<Notification> GetAllNotificationsAsync();
    Task<ICollection<Notification>> GetNotificationsByUserIdAsync(Guid userId, CancellationToken token);

    Task<ICollection<Notification>> GetPaginatedUserNotifications(Guid userId, int PageSize, int PageNumber,
        CancellationToken token);
}