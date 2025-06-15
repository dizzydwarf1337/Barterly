using Domain.Entities.Users;

namespace Application.Interfaces
{
    public interface IUserActivityService
    {
        Task<UserActivitySummary> CreateUserActivity(Guid userId);
        Task<UserActivitySummary> SummarizeUserActivity(Guid userId);

    }
}
