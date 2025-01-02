using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.General
{
    public interface IGlobalNotificationQueryRepository
    {
        Task<ICollection<GlobalNotification>> GetGlobalNotificationsAsync();
        Task<GlobalNotification> GetGlobalNotificationByIdAsync(Guid id);
    }
}
