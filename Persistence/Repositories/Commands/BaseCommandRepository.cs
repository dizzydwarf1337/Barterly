using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands
{
    public class BaseCommandRepository<TContext> where TContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        internal readonly TContext _context;
        public BaseCommandRepository(TContext context)
        {
            this._context = context;
        }
    }
}
