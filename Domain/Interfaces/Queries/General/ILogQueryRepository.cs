using Domain.Entities.Common;

namespace Domain.Interfaces.Queries.General;

public interface ILogQueryRepository
{
    Task<Log> GetLogByIdAsync(Guid id, CancellationToken token);
    IQueryable<Log> GetLogs();
}