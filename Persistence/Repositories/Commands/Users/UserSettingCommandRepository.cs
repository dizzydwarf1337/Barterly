using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users;

public class UserSettingCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserSettingCommandRepository
{
    public UserSettingCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateUserSettings(UserSettings settings, CancellationToken token)
    {
        await _context.UserSettings.AddAsync(settings, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteUserSettings(Guid settingsId, CancellationToken token)
    {
        var userSettings = await _context.UserSettings.FindAsync(settingsId, token) ??
                           throw new EntityNotFoundException("User settings");
        _context.UserSettings.Remove(userSettings);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateUserSettings(UserSettings settings, CancellationToken token)
    {
        _context.UserSettings.Update(settings);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateUserSettingsByUserId(Guid userId, bool isHidden, bool isDeleted, bool isBanned, bool isChatRestricted,
        bool isOpinionRestricted, bool isPostRestricted, CancellationToken token)
    {
        var settings = await _context.UserSettings.Where(x => x.UserId == userId).FirstOrDefaultAsync(token);
        if(settings == null)
            throw new EntityNotFoundException("User settings");
        settings.IsHidden = isHidden;
        settings.IsDeleted = isDeleted;
        settings.IsBanned = isBanned;
        settings.IsChatRestricted = isChatRestricted;
        settings.IsOpinionRestricted = isOpinionRestricted;
        settings.IsPostRestricted = isPostRestricted;
        await _context.SaveChangesAsync(token);
    }
}