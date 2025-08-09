using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class SearchHistoryQueryRepository : BaseQueryRepository<BarterlyDbContext>, ISearchHistoryQueryRepository
{
    public SearchHistoryQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public IQueryable<SearchHistory> GetAllSearchHistories()
    {
        return _context.SearchHistories;
    }

    public async Task<ICollection<SearchHistory>> GetSearchHistoriesByUserIdAsync(Guid userId,
        CancellationToken token)
    {
        return await _context.SearchHistories.Where(x => x.UserId == userId).ToListAsync(token);
    }

    public async Task<SearchHistory> GetSearchHistoryByIdAsync(Guid searchId, CancellationToken token)
    {
        return await _context.SearchHistories.FindAsync(searchId, token) ??
               throw new EntityNotFoundException("Search history");
    }
}