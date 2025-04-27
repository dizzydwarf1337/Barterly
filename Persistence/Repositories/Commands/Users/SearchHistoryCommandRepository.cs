
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class SearchHistoryCommandRepository : BaseCommandRepository<BarterlyDbContext>, ISearchHistoryCommandRepository
    {
        public SearchHistoryCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task AddSearchHistoryAsync(SearchHistory searchHistory)
        {
            await _context.SearchHistories.AddAsync(searchHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSearchHistory(Guid searchHistoryId)
        {
            var searchHistory = await _context.SearchHistories.FindAsync(searchHistoryId) ?? throw new EntityNotFoundException("Search history");
            _context.SearchHistories.Remove(searchHistory);
            await _context.SaveChangesAsync();
        }
    }
}
