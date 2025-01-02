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
    public class SessionFavouriteRepository : BaseRepository, ISessionFavouriteCommandRepository, ISessionFavouriteQueryRepository
    {
        public SessionFavouriteRepository(BarterlyDbContext context) : base(context) { }
        public async Task AddSessionFavouriteAsync(SessionFavouritePost sessionFavourite)
        {
            await _context.SessionFavouritePosts.AddAsync(sessionFavourite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSessionFavourite(Guid sessionFavouriteId)
        {
            var sessionFavourite = await GetSessionFavouritePostByIdAsync(sessionFavouriteId);
           _context.SessionFavouritePosts.Remove(sessionFavourite);
           await _context.SaveChangesAsync();
        }

        public async Task<SessionFavouritePost> GetSessionFavouritePostByIdAsync(Guid sessionFavouriteId)
        {
            return await _context.SessionFavouritePosts.FindAsync(sessionFavouriteId) ?? throw new Exception("Session favourite not found");
        }

        public async Task<ICollection<SessionFavouritePost>> GetSessionFavouritesBySessionIdAsync(Guid sessionId)
        {
            return await _context.SessionFavouritePosts.Where(x => x.SessionId == sessionId).ToListAsync();
        }
    }
}
