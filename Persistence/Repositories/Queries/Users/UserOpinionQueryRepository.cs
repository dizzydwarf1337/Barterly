using Domain.Entities.Users;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class UserOpinionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserOpinionQueryRepository
    {
        public UserOpinionQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<UserOpinion> GetUserOpinionByIdAsync(Guid id)
        {
            return await _context.UserOpinions.FindAsync(id) ?? throw new Exception("User opinion not found");
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsAsync()
        {
            return await _context.UserOpinions.ToListAsync();
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsByAuthorIdAsync(Guid authorId)
        {
            return await _context.UserOpinions.Where(x => x.AuthorId == authorId).ToListAsync();
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsByUserIdAsync(Guid userId)
        {
            return await _context.UserOpinions.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<UserOpinion>> GetUserOpinionsPaginated(Guid userId, int page, int pageSize)
        {
            return await _context.UserOpinions.Skip((page - 1) * pageSize).Take(pageSize).Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
