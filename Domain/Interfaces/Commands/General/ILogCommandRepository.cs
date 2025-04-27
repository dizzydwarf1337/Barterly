using Domain.Entities.Common;
using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.General
{
    public interface ILogCommandRepository
    {
        Task CreateLogAsync(Log log);
        Task ChangeLogType(Guid id, LogType logType);
        Task DeleteLogAsync(Guid id);
        Task DeleteLogRangeAsync(ICollection<Log> logs);
        Task DeleteUsersLogs(Guid userId);
        Task DeletePostsLogs(Guid postId);

    }
}
