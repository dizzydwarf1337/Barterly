using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Commands.General;
using Persistence.Database;

namespace Persistence.Repositories.Commands.General
{
    public class LogCommandRepository : BaseCommandRepository<BarterlyDbContext>, ILogCommandRepository
    {
        public LogCommandRepository(BarterlyDbContext context) : base(context) { }

        public async Task ChangeLogType(Guid id, LogType logType)
        {
            var log = await _context.Logs.FindAsync(id) ?? throw new Exception("Log not found");
            log.LogType = logType;
            await _context.SaveChangesAsync();
        }

        public async Task CreateLogAsync(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLogAsync(Guid id)
        {
            var log = await _context.Logs.FindAsync(id) ?? throw new Exception("Log not found");
            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLogRangeAsync(ICollection<Log> logs)
        {
            _context.Logs.RemoveRange(logs);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostsLogs(Guid postId)
        {
            var logs = _context.Logs.Where(x => x.PostId == postId);
            _context.Logs.RemoveRange(logs);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsersLogs(Guid userId)
        {
            var logs = _context.Logs.Where(x => x.UserId == userId);
            _context.Logs.RemoveRange(logs);
            await _context.SaveChangesAsync();
        }
    }
}
