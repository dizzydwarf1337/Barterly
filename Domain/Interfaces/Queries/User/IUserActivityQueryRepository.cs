using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface IUserActivityQueryRepository
{
    IQueryable<UserActivitySummary> GetUserActivities();
    Task<UserActivitySummary?> GetUserActivityByIdAsync(Guid id, CancellationToken token);
    Task<UserActivitySummary> GetUserActivityByUserIdAsync(Guid userId, CancellationToken token);
}