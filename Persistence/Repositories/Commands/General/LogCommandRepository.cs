using Domain.Entities.Common;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.General;
using Persistence.Database;

namespace Persistence.Repositories.Commands.General;

public class LogCommandRepository : BaseCommandRepository<BarterlyDbContext>, ILogCommandRepository
{
    public LogCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task ChangeLogType(Guid id, LogType logType, CancellationToken token)
    {
        var log = await _context.Logs.FindAsync(id, token) ?? throw new EntityNotFoundException("Log");
        log.LogType = logType;
        await _context.SaveChangesAsync(token);
    }

    public async Task CreateLogAsync(Log log, CancellationToken token)
    {
        await _context.Logs.AddAsync(log, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteLogAsync(Guid id, CancellationToken token)
    {
        var log = await _context.Logs.FindAsync(id, token) ?? throw new EntityNotFoundException("Log");
        _context.Logs.Remove(log);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteLogRangeAsync(ICollection<Log> logs, CancellationToken token)
    {
        _context.Logs.RemoveRange(logs);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeletePostsLogs(Guid postId, CancellationToken token)
    {
        var logs = _context.Logs.Where(x => x.PostId == postId);
        _context.Logs.RemoveRange(logs);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteUsersLogs(Guid userId, CancellationToken token)
    {
        var logs = _context.Logs.Where(x => x.UserId == userId);
        _context.Logs.RemoveRange(logs);
        await _context.SaveChangesAsync(token);
    }
}