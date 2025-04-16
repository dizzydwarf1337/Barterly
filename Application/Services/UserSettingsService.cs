using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
             await _userSettingCommandRepository.SetBanStatusAsync(userId, isBanned);
        }

        public async Task SetChatRestrictionAsync(Guid userId, bool isRestricted)
        {
            await _userSettingCommandRepository.SetChatRestrictionAsync(userId, isRestricted);
        }

        public async Task SetHiddenAsync(Guid userId, bool isHidden)
        {
            await _userSettingCommandRepository.SetHiddenAsync(userId, isHidden);
        }

        public async Task SetOpinionRestrictionAsync(Guid userId, bool isRestricted)
        {
            await _userSettingCommandRepository.SetOpinionRestrictionAsync(userId, isRestricted);
        }

        public async Task SetPostRestrictionAsync(Guid userId, bool isRestricted)
        {
            await _userSettingCommandRepository.SetPostRestrictionAsync(userId, isRestricted);
        }
    }
}
