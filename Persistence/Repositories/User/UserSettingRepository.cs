using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class UserSettingRepository : BaseRepository, IUserSettingCommandRepository, IUserSettingQueryRepository
    {
        public UserSettingRepository(BarterlyDbContext context) : base(context) { }

        public async Task<UserSetting> GetUserSettingByIdAsync(Guid id)
        {
            return await _context.UserSettings.FindAsync(id) ?? throw new Exception("User setting not found");  
        }

        public async Task<UserSetting> GetUserSettingByUserIdAsync(Guid userId)
        {
            return await _context.UserSettings.FirstOrDefaultAsync(x=>x.UserId==userId) ?? throw new Exception("User setting not found");
        }

        public async Task<ICollection<UserSetting>> GetUserSettingsAsync()
        {
            return await _context.UserSettings.ToListAsync();
        }

        public async Task SetBanStatusAsync(Guid userId, bool isBanned)
        {
           var userSettings = await GetUserSettingByUserIdAsync(userId);
            userSettings.IsBanned = isBanned;
            await _context.SaveChangesAsync();
        }

        public async Task SetChatRestrictionAsync(Guid userId, bool isRestricted)
        {
            var userSettings = await GetUserSettingByUserIdAsync(userId);
            userSettings.IsChatRestricted = isRestricted;
            await _context.SaveChangesAsync();
        }

        public async Task SetHiddenAsync(Guid userId, bool isHidden)
        {
            var userSettings = await GetUserSettingByUserIdAsync(userId);
            userSettings.IsHidden = isHidden;
            await _context.SaveChangesAsync();
        }

        public async Task SetOpinionRestrictionAsync(Guid userId, bool isRestricted)
        {
            var userSettings = await GetUserSettingByUserIdAsync(userId);
            userSettings.IsOpinionRestricted = isRestricted;
            await _context.SaveChangesAsync();
        }

        public async Task SetPostRestrictionAsync(Guid userId, bool isRestricted)
        {
            var userSettings = await GetUserSettingByUserIdAsync(userId);
            userSettings.IsPostRestricted = isRestricted;
            await _context.SaveChangesAsync();
        }
    }
}
