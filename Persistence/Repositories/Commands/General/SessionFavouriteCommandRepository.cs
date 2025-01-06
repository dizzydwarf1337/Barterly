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
    public class SessionFavouriteCommandRepository : BaseCommandRepository<BarterlyDbContext>, ISessionFavouriteCommandRepository
    {
        public SessionFavouriteCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task AddSessionFavouriteAsync(SessionFavouritePost sessionFavourite)
        {
            await _context.SessionFavouritePosts.AddAsync(sessionFavourite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSessionFavourite(Guid sessionFavouriteId)
        {
            var sessionFavourite = await _context.SessionFavouritePosts.FindAsync(sessionFavouriteId) ?? throw new Exception("Session favourite not found");
            _context.SessionFavouritePosts.Remove(sessionFavourite);
            await _context.SaveChangesAsync();
        }
    }
}
