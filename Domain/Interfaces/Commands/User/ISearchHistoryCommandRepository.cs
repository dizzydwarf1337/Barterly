using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface ISearchHistoryCommandRepository
    {
        Task AddSearchHistoryAsync(SearchHistory searchHistory);
        Task DeleteSearchHistory(Guid searchHistoryId);
    }
}
