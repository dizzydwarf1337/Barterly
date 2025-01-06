using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries
{
    public class BaseQueryRepository<TContext>
    {
        internal readonly TContext _context;
        public BaseQueryRepository(TContext context)
        {
            this._context = context;
        }
    }
}
