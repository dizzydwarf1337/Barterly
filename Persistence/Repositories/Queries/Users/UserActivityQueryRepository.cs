using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class UserActivityQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserActivityQueryRepository
{
    public UserActivityQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public IQueryable<UserActivitySummary> GetUserActivities()
    {
        return _context.UserActivities;
    }

    public async Task<UserActivitySummary?> GetUserActivityByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.UserActivities.FindAsync(id, token);
    }

    public async Task<UserActivitySummary> GetUserActivityByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.UserActivities.FirstOrDefaultAsync(x => x.UserId == userId, token) ??
               throw new EntityNotFoundException("User activity summary ");
    }
}