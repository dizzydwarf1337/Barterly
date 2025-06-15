using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class UserActivityQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserActivityQueryRepository
    {
        public UserActivityQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<UserActivitySummary>> GetUserActivitiesAsync()
        {
            return await _context.UserActivities.ToListAsync();
        }

        public async Task<UserActivitySummary?> GetUserActivityByIdAsync(Guid id)
        {
            return await _context.UserActivities.FindAsync(id);
        }

        public async Task<UserActivitySummary> GetUserActivityByUserIdAsync(Guid userId)
        {
            return await _context.UserActivities.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new EntityNotFoundException("User activity summary ");
        }
    }
}
