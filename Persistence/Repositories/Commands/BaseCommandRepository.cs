using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence.Repositories.Commands;

public class BaseCommandRepository<TContext> where TContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    internal readonly TContext _context;

    public BaseCommandRepository(TContext context)
    {
        _context = context;
    }
}