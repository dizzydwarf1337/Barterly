using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserSettingCommandRepository
    {
        Task CreateUserSettings(UserSettings settings);
        Task DeleteUserSettings(Guid settingsId);
        Task SetHiddenAsync(Guid userId, bool isHidden);
        Task SetBanStatusAsync(Guid userId, bool isBanned);
        Task SetPostRestrictionAsync(Guid userId, bool isRestricted);
        Task SetOpinionRestrictionAsync(Guid userId, bool isRestricted);
        Task SetChatRestrictionAsync(Guid userId, bool isRestricted);
    }
}
