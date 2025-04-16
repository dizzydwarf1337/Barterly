using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface ISearchHistoryCommandRepository
    {
        Task AddSearchHistoryAsync(SearchHistory searchHistory);
        Task DeleteSearchHistory(Guid searchHistoryId);
    }
}
