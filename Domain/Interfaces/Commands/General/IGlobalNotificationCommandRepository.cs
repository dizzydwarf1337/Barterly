using Domain.Entities.Common;

namespace Domain.Interfaces.Commands.General
{
    public interface IGlobalNotificationCommandRepository
    {
        Task CreateGlobalNotificationAsync(GlobalNotification globalNotification);
        Task DeleteGlobalNotificationAsync(Guid id);
    }
}
