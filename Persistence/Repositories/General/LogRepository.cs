using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.General
{
    public class LogRepository : BaseRepository,ILogCommandRepository, ILogQueryRepository
    {
        public LogRepository(BarterlyDbContext context) : base(context) { }

        public async Task ChangeLogType(Guid id, LogType logType)
        {
            var log = await GetLogByIdAsync(id);
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
            var log = await GetLogByIdAsync(id);
            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task<Log> GetLogByIdAsync(Guid id)
        {
            return await _context.Logs.FindAsync(id) ?? throw new Exception($"Log with id {id} not found.");
        }

        public async Task<ICollection<Log>> GetLogsAsync()
        {
            return await _context.Logs.ToListAsync() ?? throw new Exception("No logs found.");
        }

        public async Task<ICollection<Log>> GetLogsByPostIdAsync(Guid postId)
        {
            return await _context.Logs.Where(l => l.PostId == postId).ToListAsync() ?? throw new Exception($"No logs found for post with id {postId}.");
        }

        public async Task<ICollection<Log>> GetLogsByUserIdAsync(Guid userId)
        {
            return await _context.Logs.Where(l => l.UserId == userId).ToListAsync() ?? throw new Exception($"No logs found for user with id {userId}.");
        }
    }
}
