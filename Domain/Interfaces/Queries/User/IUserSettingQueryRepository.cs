using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserSettingQueryRepository
    {
        Task<UserSettings> GetUserSettingByIdAsync(Guid id);
        Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId);
    }
}
