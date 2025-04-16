using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
