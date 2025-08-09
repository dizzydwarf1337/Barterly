using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post;

public class PostImageCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostImageCommandRepository
{
    public PostImageCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreatePostImageAsync(PostImage postImage, CancellationToken token)
    {
        await _context.PostImages.AddAsync(postImage, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeletePostImageAsync(Guid id, CancellationToken token)
    {
        var postImage = await _context.PostImages.FindAsync(id, token) ??
                        throw new EntityNotFoundException($"PostImage with id: {id}");
        _context.PostImages.Remove(postImage);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeletePostImagesByPostIdAsync(Guid postId, CancellationToken token)
    {
        await _context.PostImages
            .Where(x => x.PostId == postId)
            .ExecuteDeleteAsync(token);
        await _context.SaveChangesAsync(token);
    }
}