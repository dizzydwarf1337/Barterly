using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User;

public interface IUserActivityCommandRepository
{
    Task CreateUserActivitySummary(UserActivitySummary userActivitySummary, CancellationToken token);
    Task DeleteUserActivitySummary(Guid SummaryId, CancellationToken token);
    Task UpdateUserActivitySummary(UserActivitySummary userActivitySummary, CancellationToken token);
}