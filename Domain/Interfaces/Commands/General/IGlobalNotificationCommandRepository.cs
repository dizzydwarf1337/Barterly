using Domain.Entities.Common;

namespace Domain.Interfaces.Commands.General;

public interface IGlobalNotificationCommandRepository
{
    Task CreateGlobalNotificationAsync(GlobalNotification globalNotification, CancellationToken token);
    Task DeleteGlobalNotificationAsync(Guid id, CancellationToken token);
}