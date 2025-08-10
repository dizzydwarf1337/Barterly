using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User;

public interface INotificationCommandRepository
{
    Task CreateNotificationAsync(Notification notification, CancellationToken token);
    Task UpdateNotificationAsync(Notification notification, CancellationToken token);
    Task SetReadNotificationAsync(Guid id, bool IsRead, CancellationToken token);
    Task DeleteNotificationAsync(Guid id, CancellationToken token);
}