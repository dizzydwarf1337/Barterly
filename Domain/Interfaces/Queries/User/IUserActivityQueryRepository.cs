using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserActivityQueryRepository
    {
        Task<ICollection<UserActivitySummary>> GetUserActivitiesAsync();
        Task<UserActivitySummary> GetUserActivityByIdAsync(Guid id);
        Task<UserActivitySummary> GetUserActivityByUserIdAsync(Guid userId);
    }
}
