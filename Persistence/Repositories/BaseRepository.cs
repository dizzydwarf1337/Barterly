using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BaseRepository
    {
        internal readonly BarterlyDbContext _context;
        public BaseRepository(BarterlyDbContext context)
        {
            this._context = context;
        }
    }
}
