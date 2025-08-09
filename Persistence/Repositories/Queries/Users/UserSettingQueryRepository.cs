using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class UserSettingQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserSettingQueryRepository
{
    public UserSettingQueryRepository(BarterlyDbContext context) : base(context)
    {
    }


    public async Task<UserSettings> GetUserSettingByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.UserSettings.FindAsync(id, token) ??
               throw new EntityNotFoundException("User setting");
    }

    public async Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId, token) ??
               throw new EntityNotFoundException("User setting");
    }
}