using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

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
            var userSettings = await _context.UserSettings.FindAsync(settingsId) ?? throw new EntityNotFoundException("User settings");
            _context.UserSettings.Remove(userSettings);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserSettings(UserSettings settings)
        {
            _context.UserSettings.Update(settings);
            await _context.SaveChangesAsync();
        }
    }
}
