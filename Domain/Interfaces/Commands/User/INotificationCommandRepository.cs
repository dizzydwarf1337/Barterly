using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface INotificationCommandRepository
    {
        Task CreateNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task SetReadNotificationAsync(Guid id, bool IsRead);
        Task DeleteNotificationAsync(Guid id);
    }
}
