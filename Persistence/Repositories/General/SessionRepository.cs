using Domain.Entities;
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
    public class SessionRepository : BaseRepository, ISessionCommandRepository, ISessionQueryRepository
    {
        public SessionRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddSessionAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(Guid sessionId)
        {
            var session = await GetSessionByIdAsync(sessionId);
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Session>> GetAllSessionsAsync()
        {
            return await _context.Sessions.ToListAsync();
        }

        public async Task<Session> GetSessionByIdAsync(Guid sessionId)
        {
           return await _context.Sessions.FindAsync(sessionId) ?? throw new Exception("Session not found");
            
        }
        public async Task UpdateSessionAsync(Session session)
        {
           _context.Sessions.Update(session);
           await _context.SaveChangesAsync();
        }
    }
}
