using Domain.Entities;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.General
{
    public class LogQueryRepository : BaseQueryRepository<BarterlyDbContext>, ILogQueryRepository
    {
        public LogQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<Log> GetLogByIdAsync(Guid id)
        {
            return await _context.Logs.FindAsync(id) ?? throw new Exception($"Log with id {id} not found.");
        }

        public async Task<ICollection<Log>> GetLogsPaginatedAsync(int PageSize,int PageNum)
        {
            return await _context.Logs.Skip(PageSize*PageNum).Take(PageSize).ToListAsync() ?? throw new Exception("No logs found.");
        }

        public async Task<ICollection<Log>> GetLogsByPostIdAsync(Guid postId)
        {
            return await _context.Logs.Where(l => l.PostId == postId).ToListAsync();
        }

        public async Task<ICollection<Log>> GetLogsByUserIdAsync(Guid userId)
        {
            return await _context.Logs.Where(l => l.UserId == userId).ToListAsync();
        }
    }
}
