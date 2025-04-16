using Domain.Entities.Users;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class SearchHistoryQueryRepository : BaseQueryRepository<BarterlyDbContext>, ISearchHistoryQueryRepository
    {
        public SearchHistoryQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<SearchHistory>> GetAllSearchHistoriesAsync()
        {
            return await _context.SearchHistories.ToListAsync();
        }

        public async Task<ICollection<SearchHistory>> GetSearchHistoriesByUserIdAsync(Guid userId)
        {
            return await _context.SearchHistories.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<SearchHistory> GetSearchHistoryByIdAsync(Guid searchId)
        {
            return await _context.SearchHistories.FindAsync(searchId) ?? throw new Exception("Search history by id not found");
        }
    }
}
