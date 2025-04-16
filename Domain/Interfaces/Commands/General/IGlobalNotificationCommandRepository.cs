using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.General
{
    public interface IGlobalNotificationCommandRepository
    {
        Task CreateGlobalNotificationAsync(GlobalNotification globalNotification);
        Task DeleteGlobalNotificationAsync(Guid id);
    }
}
