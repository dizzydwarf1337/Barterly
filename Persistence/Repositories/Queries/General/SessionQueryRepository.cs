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
    public class SessionQueryRepository : BaseQueryRepository<BarterlyDbContext>, ISessionQueryRepository
    {
        public SessionQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Session>> GetAllSessionsAsync()
        {
            return await _context.Sessions.ToListAsync();
        }

        public async Task<Session> GetSessionByIdAsync(Guid sessionId)
        {
            return await _context.Sessions.FindAsync(sessionId) ?? throw new Exception("Session not found");
        }
    }
}
