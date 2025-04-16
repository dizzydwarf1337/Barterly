using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface ISearchHistoryQueryRepository
    {
        Task<SearchHistory> GetSearchHistoryByIdAsync(Guid searchId);
        Task<ICollection<SearchHistory>> GetAllSearchHistoriesAsync();
        Task<ICollection<SearchHistory>> GetSearchHistoriesByUserIdAsync(Guid userId);

    }
}
