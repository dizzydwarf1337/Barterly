using Domain.Entities.Common;
using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.General;

public interface ILogCommandRepository
{
    Task CreateLogAsync(Log log, CancellationToken token);
    Task ChangeLogType(Guid id, LogType logType, CancellationToken token);
    Task DeleteLogAsync(Guid id, CancellationToken token);
    Task DeleteLogRangeAsync(ICollection<Log> logs, CancellationToken token);
    Task DeleteUsersLogs(Guid userId, CancellationToken token);
    Task DeletePostsLogs(Guid postId, CancellationToken token);
}