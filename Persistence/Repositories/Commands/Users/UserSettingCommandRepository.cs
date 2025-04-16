using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class UserSettingCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserSettingCommandRepository
    {
        public UserSettingCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateUserSettings(UserSettings settings)
        {
            await _context.UserSettings.AddAsync(settings);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserSettings(Guid settingsId)
        {
            var userSettings = await _context.UserSettings.FindAsync(settingsId);
            if (userSettings != null)
            {
                throw new ArgumentNullException(nameof(userSettings.Id));
            }
             _context.UserSettings.Remove(userSettings);
            await _context.SaveChangesAsync();
        }

        public async Task SetBanStatusAsync(Guid userId, bool isBanned)
        {
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x=>x.UserId == userId) ?? throw new Exception("User Settings not found") ;
            userSettings.IsBanned = isBanned;
            await _context.SaveChangesAsync();
        }

        public async Task SetChatRestrictionAsync(Guid userId, bool isRestricted)
        {
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("User Settings not found");
            userSettings.IsChatRestricted = isRestricted;
            await _context.SaveChangesAsync();
        }

        public async Task SetHiddenAsync(Guid userId, bool isHidden)
        {
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("User Settings not found");
            userSettings.IsHidden = isHidden;
            await _context.SaveChangesAsync();
        }

        public async Task SetOpinionRestrictionAsync(Guid userId, bool isRestricted)
        {
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("User Settings not found");
            userSettings.IsOpinionRestricted = isRestricted;
            await _context.SaveChangesAsync();
        }

        public async Task SetPostRestrictionAsync(Guid userId, bool isRestricted)
        {
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("User Settings not found");
            userSettings.IsPostRestricted = isRestricted;
            await _context.SaveChangesAsync();
        }
    }
}
