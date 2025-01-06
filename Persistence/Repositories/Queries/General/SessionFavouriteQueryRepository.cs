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
    public class SessionFavouriteQueryRepository : BaseQueryRepository<BarterlyDbContext>, ISessionFavouriteQueryRepository
    {
        public SessionFavouriteQueryRepository(BarterlyDbContext context) : base(context)
        {
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
