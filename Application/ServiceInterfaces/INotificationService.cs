using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface INotificationService
    {
        Task ReadNotification(Guid notificationId);
        Task DeleteGlobalNotification(Guid notificationId);
        Task DeleteNotification(Guid notificationId);
        Task SendNotification(NotificationDto notification);
        Task<NotificationDto> GetNotification(Guid notificationId);
        Task<NotificationDto> GetGlobalNotification(Guid notificationId);
        Task<ICollection<NotificationDto>> GetPaginatedUserNotification(Guid userId, int PageSize, int PageNumber);

    }
}
