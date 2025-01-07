using Domain.Entities;
using Domain.Interfaces.Commands.General;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.General
{
    public class SessionCommandRepository : BaseCommandRepository<BarterlyDbContext>, ISessionCommandRepository
    {
        public SessionCommandRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddSessionAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(Guid sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId) ?? throw new Exception("Session not found");
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task ExtendSession(Guid sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId) ?? throw new Exception("Session not found");
            session.Expired = session.Expired.AddDays(1);
        }

        public async Task UpdateSessionAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
