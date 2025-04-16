using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserSettingsService
    {
        Task CreateUserSettings(UserSettings settings);
        Task DeleteUserSettings(Guid settingsId);
        Task SetHiddenAsync(Guid userId, bool isHidden);
        Task SetBanStatusAsync(Guid userId, bool isBanned);
        Task SetPostRestrictionAsync(Guid userId, bool isRestricted);
        Task SetOpinionRestrictionAsync(Guid userId, bool isRestricted);
        Task SetChatRestrictionAsync(Guid userId, bool isRestricted);
        Task<UserSettings> GetUserSettingByIdAsync(Guid id);
        Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId);
    }
}
