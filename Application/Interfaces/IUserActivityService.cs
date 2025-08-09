using Domain.Entities.Users;

namespace Application.Interfaces;

public interface IUserActivityService
{
    Task<UserActivitySummary> CreateUserActivity(Guid userId, CancellationToken token);
    Task<UserActivitySummary> SummarizeUserActivity(Guid userId, CancellationToken token);
}