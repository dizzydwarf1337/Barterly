using Domain.Entities.Common;

namespace Domain.Interfaces.Queries.General
{
    public interface ILogQueryRepository
    {
        Task<Log> GetLogByIdAsync(Guid id);
        Task<ICollection<Log>> GetLogsPaginatedAsync(int PageSize, int PageNum);
        Task<ICollection<Log>> GetLogsByUserIdAsync(Guid userId);
        Task<ICollection<Log>> GetLogsByPostIdAsync(Guid postId);
    }
}
