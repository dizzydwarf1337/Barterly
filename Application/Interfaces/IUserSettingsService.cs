using Domain.Entities.Users;

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
