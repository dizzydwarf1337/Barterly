using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Repositories.Queries;

public class PostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostQueryRepository
{
    public PostQueryRepository(BarterlyDbContext context) : base(context)
    {
    }


    public async Task<Post> GetPostById(Guid postId, CancellationToken token)
    {
        var post = await _context.Posts
            .Include(x => x.PostSettings)
            .Include(x => x.PostImages)
            .Include(x => x.PostOpinions)
            .Include(x => x.Promotion)
            .Include(x => x.SubCategory)
            .FirstOrDefaultAsync(x => x.Id == postId);

        if (post == null || post.PostSettings == null) throw new EntityNotFoundException($"Post with id {postId}");

        return post;
    }

    public IQueryable<Post> GetAllPosts()
    {
        return _context.Posts
            .Include(x => x.PostSettings)
            .Include(x => x.Promotion)
            .Include(x => x.SubCategory);
    }
}