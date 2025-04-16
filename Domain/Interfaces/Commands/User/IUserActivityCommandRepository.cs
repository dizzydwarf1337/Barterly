using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserActivityCommandRepository
    {
        Task CreateUserActivitySummary(UserActivitySummary userActivitySummary);
        Task DeleteUserActivitySummary(Guid SummaryId);
        Task UpdateUserActivitySummary(UserActivitySummary userActivitySummary);
    }
}
