using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post
{
    public class PostOpinionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostOpinionQueryRepository
    {
        public PostOpinionQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<PostOpinion> GetPostOpinionByIdAsync(Guid id)
        {
            return await _context.PostOpinions.FindAsync(id) ?? throw new EntityNotFoundException("Post opinion");
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsAsync()
        {
            return await _context.PostOpinions.ToListAsync();
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsByAuthorIdAsync(Guid userId)
        {
            return await _context.PostOpinions.Where(x => x.AuthorId == userId).ToListAsync();
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsByPostIdAsync(Guid postId)
        {
            return await _context.PostOpinions.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<ICollection<PostOpinion>> GetPostOpinionsPaginatedAsync(Guid postId, int page, int pageSize)
        {
            return await _context.PostOpinions.Skip((page - 1) * pageSize).Take(pageSize).Where(x => x.PostId == postId).ToListAsync();
        }

    }
}
