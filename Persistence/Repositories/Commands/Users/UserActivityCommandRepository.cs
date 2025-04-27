
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class UserActivityCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserActivityCommandRepository
    {
        public UserActivityCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateUserActivitySummary(UserActivitySummary userActivitySummary)
        {
            await _context.UserActivities.AddAsync(userActivitySummary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserActivitySummary(Guid userId)
        {
            var userActivity = await _context.UserActivities.FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new EntityNotFoundException("User Activity");
            _context.UserActivities.Remove(userActivity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserActivitySummary(UserActivitySummary userActivitySummary)
        {
            _context.UserActivities.Update(userActivitySummary);
            await _context.SaveChangesAsync();
        }
    }
}
