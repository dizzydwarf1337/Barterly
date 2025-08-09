using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Posts;

public class PostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostCommandRepository
{
    public PostCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<Domain.Entities.Posts.Post> CreatePostAsync(Domain.Entities.Posts.Post post,
        CancellationToken token)
    {
        await _context.Posts.AddAsync(post, token);
        await _context.SaveChangesAsync(token);
        var createdPost = await _context.Posts
                              .Include(x => x.Promotion)
                              .Include(x => x.SubCategory)
                              .Include(x => x.PostImages)
                              .Include(x => x.PostSettings)
                              .FirstOrDefaultAsync(x => x.Id == post.Id, token) ??
                          throw new EntityCreatingException("Post", "PostCommandRepository");

        return createdPost;
    }

    public async Task<Domain.Entities.Posts.Post> UpdatePostAsync(Domain.Entities.Posts.Post post,
        CancellationToken token)
    {
        var existingPost = await _context.Posts
                               .Include(x => x.Promotion)
                               .Include(x => x.SubCategory)
                               .Include(x => x.PostImages)
                               .Include(x => x.PostSettings)
                               .FirstOrDefaultAsync(x => x.Id == post.Id, token)
                           ?? throw new EntityNotFoundException("Post");

        _context.Entry(existingPost).CurrentValues.SetValues(post);
        existingPost.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(token);
        return existingPost;
    }

    public async Task UpdatePostMainImage(Guid postId, string mainImageUrl, CancellationToken token)
    {
        var post = await _context.Posts.FindAsync(postId, token) ?? throw new EntityNotFoundException("Post");
        post.MainImageUrl = mainImageUrl;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync(token);
    }

    public async Task IncreasePostView(Guid postId, CancellationToken token)
    {
        var post = await _context.Posts.FindAsync(postId, token) ?? throw new EntityNotFoundException("Post");
        post.ViewsCount++;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync(token);
    }
}