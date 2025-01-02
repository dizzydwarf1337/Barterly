using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface ISearchHistoryQueryRepository
    {
        Task<SearchHistory> GetSearchHistoryByIdAsync(Guid searchId);
        Task<ICollection<SearchHistory>> GetAllSearchHistoriesAsync();
        Task<ICollection<SearchHistory>> GetSearchHistoriesByUserIdAsync(Guid userId);

    }
}
