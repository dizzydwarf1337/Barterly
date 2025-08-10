using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface ISearchHistoryQueryRepository
{
    Task<SearchHistory> GetSearchHistoryByIdAsync(Guid searchId, CancellationToken token);
    IQueryable<SearchHistory> GetAllSearchHistories();
    Task<ICollection<SearchHistory>> GetSearchHistoriesByUserIdAsync(Guid userId, CancellationToken token);
}