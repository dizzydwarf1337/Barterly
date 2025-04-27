using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post
{
    public class PostImageQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostImageQueryRepository
    {
        public PostImageQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<PostImage> GetPostImageAsync(Guid id)
        {
            return await _context.PostImages.FindAsync(id) ?? throw new EntityNotFoundException("Post image");
        }

        public async Task<ICollection<PostImage>> GetPostImagesAsync()
        {
            return await _context.PostImages.AsNoTracking().ToListAsync();
        }

        public async Task<ICollection<PostImage>> GetPostImagesByPostIdAsync(Guid postId)
        {
            return await _context.PostImages.Where(x => x.PostId == postId).AsNoTracking().ToListAsync();
        }
    }
}
