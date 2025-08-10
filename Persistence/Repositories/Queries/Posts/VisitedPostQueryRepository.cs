using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Posts;

public class VisitedPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IVisitedPostQueryRepository
{
    public VisitedPostQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<VisitedPost> GetVisitedPostByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.VisitedPosts.FindAsync(id, token) ??
               throw new EntityNotFoundException("Visited post");
    }

    public IQueryable<VisitedPost> GetVisitedPostsAsync()
    {
        return _context.VisitedPosts;
    }

    public async Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId, CancellationToken token)
    {
        return await _context.VisitedPosts.Where(x => x.PostId == postId).ToListAsync(token);
    }

    public async Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.VisitedPosts.Where(x => x.UserId == userId).Include(x => x.Post)
            .Include(x => x.Post.SubCategory).ToListAsync(token);
    }

    public async Task<VisitedPost> GetUserVisitedPost(Guid postId, Guid userId, CancellationToken token)
    {
        return await _context.VisitedPosts.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId,
            token) ?? throw new EntityNotFoundException("Visited post");
    }
}