using Application.DTOs;
using Domain.Entities.Users;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task ReadNotification(Guid notificationId);
        Task DeleteGlobalNotification(Guid notificationId);
        Task DeleteNotification(Guid notificationId);
        Task SendNotification(Notification notification);
        Task<NotificationDto> GetNotification(Guid notificationId);
        Task<NotificationDto> GetGlobalNotification(Guid notificationId);
        Task<ICollection<NotificationDto>> GetPaginatedUserNotification(Guid userId, int PageSize, int PageNumber);
        Task SendGlobalNotification(NotificationDto globalNotification);

    }
}
