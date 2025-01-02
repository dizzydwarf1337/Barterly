using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class UserActiviryRepository : BaseRepository, IUserActivityCommandRepository, IUserActivityQueryRepository
    {
        public UserActiviryRepository(BarterlyDbContext context) : base(context) { }

        public async Task CreateUserActivitySummary(UserActivitySummary userActivitySummary)
        {
            await _context.UserActivities.AddAsync(userActivitySummary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserActivitySummary(Guid SummaryId)
        {
            var userActivity = await GetUserActivityByIdAsync(SummaryId);
            _context.UserActivities.Remove(userActivity);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<UserActivitySummary>> GetUserActivitiesAsync()
        {
            return await _context.UserActivities.ToListAsync();
        }

        public async Task<UserActivitySummary> GetUserActivityByIdAsync(Guid id)
        {
            return await _context.UserActivities.FindAsync(id);
        }

        public async Task<UserActivitySummary> GetUserActivityByUserIdAsync(Guid userId)
        {
            return await _context.UserActivities.FirstOrDefaultAsync(x=>x.UserId==userId) ?? throw new Exception("User activity summary not found");
        }

        public async Task UpdateUserActivitySummary(UserActivitySummary userActivitySummary)
        {
            _context.UserActivities.Update(userActivitySummary);
            await _context.SaveChangesAsync();
        }
    }
}
