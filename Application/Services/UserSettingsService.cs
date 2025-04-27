using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using System.Runtime.CompilerServices;

namespace Application.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUserSettingCommandRepository _userSettingCommandRepository;

        private readonly IUserSettingQueryRepository _userSettingQueryRepository;

        public UserSettingsService(IUserSettingCommandRepository userSettingCommandRepository, IUserSettingQueryRepository userSettingQueryRepository)
        {
            _userSettingCommandRepository = userSettingCommandRepository;
            _userSettingQueryRepository = userSettingQueryRepository;
        }

        public async Task CreateUserSettings(UserSettings settings)
        {
            await _userSettingCommandRepository.CreateUserSettings(settings);
        }

        public async Task DeleteUserSettings(Guid settingsId)
        {
            await _userSettingCommandRepository.DeleteUserSettings(settingsId);
        }

        public async Task<UserSettings> GetUserSettingByIdAsync(Guid id)
        {
            return await _userSettingQueryRepository.GetUserSettingByIdAsync(id);
        }

        public async Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId)
        {
            return await _userSettingQueryRepository.GetUserSettingByUserIdAsync(userId);
        }

        public async Task SetBanStatusAsync(Guid userId, bool isBanned)
        {
            var settings = await _userSettingQueryRepository.GetUserSettingByUserIdAsync(userId);
            settings.IsBanned = isBanned;
            await _userSettingCommandRepository.UpdateUserSettings(settings);
        }

        public async Task SetChatRestrictionAsync(Guid userId, bool isRestricted)
        {
            var settings = await _userSettingQueryRepository.GetUserSettingByUserIdAsync(userId);
            settings.IsChatRestricted = isRestricted;
            await _userSettingCommandRepository.UpdateUserSettings(settings);
        }

        public async Task SetHiddenAsync(Guid userId, bool isHidden)
        {
            var settings = await _userSettingQueryRepository.GetUserSettingByUserIdAsync(userId);
            settings.IsHidden = isHidden;
            await _userSettingCommandRepository.UpdateUserSettings(settings);
        }

        public async Task SetOpinionRestrictionAsync(Guid userId, bool isRestricted)
        {
            var settings = await _userSettingQueryRepository.GetUserSettingByUserIdAsync(userId);
            settings.IsOpinionRestricted = isRestricted;
            await _userSettingCommandRepository.UpdateUserSettings(settings);
        }

        public async Task SetPostRestrictionAsync(Guid userId, bool isRestricted)
        {
            var settings = await _userSettingQueryRepository.GetUserSettingByUserIdAsync(userId);
            settings.IsPostRestricted = isRestricted;
            await _userSettingCommandRepository.UpdateUserSettings(settings);
        }
    }
}
