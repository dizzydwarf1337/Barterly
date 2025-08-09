using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.General;

public class LogQueryRepository : BaseQueryRepository<BarterlyDbContext>, ILogQueryRepository
{
    public LogQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<Log> GetLogByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Logs.FindAsync(id, token) ??
               throw new EntityNotFoundException($"Log with id {id} not found.");
    }

    public IQueryable<Log> GetLogs()
    {
        return _context.Logs;
    }

    public async Task<ICollection<Log>> GetLogsPaginatedAsync(int PageSize, int PageNum, CancellationToken token)
    {
        return await _context.Logs.OrderByDescending(x => x.CreatedAt).Skip(PageSize * (PageNum - 1)).Take(PageSize)
            .ToListAsync(token);
    }

    public async Task<ICollection<Log>> GetLogsByPostIdAsync(Guid postId, CancellationToken token)
    {
        return await _context.Logs.Where(l => l.PostId == postId).ToListAsync(token);
    }

    public async Task<ICollection<Log>> GetLogsByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.Logs.Where(l => l.UserId == userId).ToListAsync(token);
    }
}