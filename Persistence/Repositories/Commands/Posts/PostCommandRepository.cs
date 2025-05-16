using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Posts
{
    public class PostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostCommandRepository
    {
        public PostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<Domain.Entities.Posts.Post> CreatePostAsync(Domain.Entities.Posts.Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            var createdPost = await _context.Posts
                .Include(x => x.Promotion)
                .Include(x => x.SubCategory)
                .Include(x => x.PostImages)
                .Include(x => x.PostSettings)
                .FirstOrDefaultAsync(x => x.Id == post.Id) ?? throw new EntityCreatingException("Post","PostCommandRepository");

            return createdPost;
        }


        public async Task<Domain.Entities.Posts.Post> UpdatePostAsync(Domain.Entities.Posts.Post post)
        {
            post.UpdatedAt = DateTime.UtcNow;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            var updatedPost = await _context.Posts
                .Include(x => x.Promotion)
                .Include(x => x.SubCategory)
                .Include(x => x.PostImages)
                .Include(x => x.PostSettings)
                .FirstOrDefaultAsync(x => x.Id == post.Id) ?? throw new EntityCreatingException("Post", "PostCommandRepository");
            return updatedPost;

        }
    }
}
