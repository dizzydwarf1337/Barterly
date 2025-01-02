using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserSettingQueryRepository
    {
        Task<ICollection<UserSetting>> GetUserSettingsAsync();
        Task<UserSetting> GetUserSettingByIdAsync(Guid id);
        Task<UserSetting> GetUserSettingByUserIdAsync(Guid userId);
    }
}
