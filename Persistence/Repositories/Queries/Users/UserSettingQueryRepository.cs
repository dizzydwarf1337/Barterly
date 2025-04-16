using Domain.Entities.Users;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class UserSettingQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserSettingQueryRepository
    {
        public UserSettingQueryRepository(BarterlyDbContext context) : base(context)
        {
        }


        public async Task<UserSettings> GetUserSettingByIdAsync(Guid id)
        {
            return await _context.UserSettings.FindAsync(id) ?? throw new Exception("User setting not found");
        }

        public async Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId)
        {
            return await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("User setting not found");
        }


    }
}
