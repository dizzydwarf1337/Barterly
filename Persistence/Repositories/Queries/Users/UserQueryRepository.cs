using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class UserQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserQueryRepository
{
    public UserQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<User> GetUserAsync(Guid id, CancellationToken token)
    {
        return await _context.Users.FindAsync(id, token) ?? throw new EntityNotFoundException("User");
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken token)
    {
        return await _context.Users.FirstOrDefaultAsync(
            x => x.NormalizedEmail == email.ToUpper(), token
        ) ?? throw new EntityNotFoundException("User");
    }

    public IQueryable<User> GetUsers()
    {
        return _context.Users;
    }
}