using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface INotificationQueryRepository
    {
        Task<Notification> GetNotificationAsync(Guid id);
        Task<ICollection<Notification>> GetAllNotificationsAsync();
        Task<ICollection<Notification>> GetNotificationsByUserIdAsync(Guid userId);
        Task<ICollection<Notification>> GetPaginatedUserNotifications(Guid userId, int PageSize, int PageNumber);
    }
}
