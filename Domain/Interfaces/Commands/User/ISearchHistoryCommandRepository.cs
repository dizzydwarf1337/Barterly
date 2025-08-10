using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User;

public interface ISearchHistoryCommandRepository
{
    Task AddSearchHistoryAsync(SearchHistory searchHistory, CancellationToken token);
    Task DeleteSearchHistory(Guid searchHistoryId, CancellationToken token);
}