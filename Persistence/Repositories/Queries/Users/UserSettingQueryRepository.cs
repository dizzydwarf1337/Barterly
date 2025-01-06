using Domain.Entities;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Users
{
    public class UserSettingQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserSettingQueryRepository
    {
        public UserSettingQueryRepository(BarterlyDbContext context) : base(context)
        {
        }


        public async Task<UserSetting> GetUserSettingByIdAsync(Guid id)
        {
            return await _context.UserSettings.FindAsync(id) ?? throw new Exception("User setting not found");
        }

        public async Task<UserSetting> GetUserSettingByUserIdAsync(Guid userId)
        {
            return await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new Exception("User setting not found");
        }

        public async Task<ICollection<UserSetting>> GetUserSettingsAsync()
        {
            return await _context.UserSettings.ToListAsync();
        }

    }
}
