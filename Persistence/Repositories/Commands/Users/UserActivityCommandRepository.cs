using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task DeleteUserActivitySummary(Guid SummaryId)
        {
            var userActivity = await _context.UserActivities.FindAsync(SummaryId) ?? throw new Exception("User Activity not found");
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
