using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post;

public class PostOpinionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostOpinionQueryRepository
{
    public PostOpinionQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<PostOpinion> GetPostOpinionByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.PostOpinions.FindAsync(id, token) ??
               throw new EntityNotFoundException("Post opinion");
    }

    public IQueryable<PostOpinion> GetPostOpinionsAsync()
    {
        return _context.PostOpinions;
    }

    public async Task<ICollection<PostOpinion>> GetPostOpinionsByAuthorIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.PostOpinions.Where(x => x.AuthorId == userId).ToListAsync(token);
    }

    public async Task<ICollection<PostOpinion>> GetPostOpinionsByPostIdAsync(Guid postId, CancellationToken token)
    {
        return await _context.PostOpinions.Where(x => x.PostId == postId).ToListAsync(token);
    }
}