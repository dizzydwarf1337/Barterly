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
    public class SearchHistoryRepository : BaseRepository, ISearchHistoryCommandRepository, ISearchHistoryQueryRepository
    {
        public SearchHistoryRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddSearchHistoryAsync(SearchHistory searchHistory)
        {
            await _context.SearchHistories.AddAsync(searchHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSearchHistory(Guid searchHistoryId)
        {
            var searchHistory = await GetSearchHistoryByIdAsync(searchHistoryId);
            _context.SearchHistories.Remove(searchHistory);
            await _context.SaveChangesAsync();
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
