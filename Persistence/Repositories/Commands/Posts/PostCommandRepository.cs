using Domain.Interfaces.Commands.Post;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Posts
{
    public class PostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostCommandRepository
    {
        public PostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreatePostAsync(Domain.Entities.Posts.Post post)
        {
            await _context.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Guid postId)
        {
            var post = await _context.Posts.FindAsync(postId) ?? throw new Exception("Post with this id doesn't exists");
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task SetHidePostAsync(Guid postId, bool IsHidden)
        {
            var post = await _context.Posts.FindAsync(postId) ?? throw new Exception("Post with this id doesn't exists");
            post.IsHidden = IsHidden;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Domain.Entities.Posts.Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}
