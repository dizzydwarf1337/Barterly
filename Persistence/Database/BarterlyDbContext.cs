using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Database
{
    public class BarterlyDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public BarterlyDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
