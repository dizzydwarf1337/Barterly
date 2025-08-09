using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class UserOpinionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserOpinionQueryRepository
{
    public UserOpinionQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<UserOpinion> GetUserOpinionByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.UserOpinions.FindAsync(id, token) ??
               throw new EntityNotFoundException("User opinion");
    }

    public IQueryable<UserOpinion> GetUserOpinionsAsync()
    {
        return _context.UserOpinions;
    }

    public async Task<ICollection<UserOpinion>> GetUserOpinionsByAuthorIdAsync(Guid authorId,
        CancellationToken token)
    {
        return await _context.UserOpinions.Where(x => x.AuthorId == authorId).ToListAsync(token);
    }

    public async Task<ICollection<UserOpinion>> GetUserOpinionsByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.UserOpinions.Where(x => x.UserId == userId).ToListAsync(token);
    }
}