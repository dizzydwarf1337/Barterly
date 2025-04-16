
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var searchHistory = await _context.SearchHistories.FindAsync(searchHistoryId) ?? throw new Exception("Search history not found");
            _context.SearchHistories.Remove(searchHistory);
            await _context.SaveChangesAsync();
        }
    }
}
