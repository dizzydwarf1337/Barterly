using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.Identity.Client;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post
{
    public class PostOpinionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostOpinionCommandRepository
    {
        public PostOpinionCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<PostOpinion> CreatePostOpinionAsync(PostOpinion opinion)
        {
            _ = await _context.Posts.FindAsync(opinion.PostId) ?? throw new EntityNotFoundException("Post");
            await _context.PostOpinions.AddAsync(opinion);
            await _context.SaveChangesAsync();
            return opinion;
        }

        public async Task DeletePostOpinionAsync(Guid id)
        {
            var opinion = await _context.PostOpinions.FindAsync(id) ?? throw new EntityNotFoundException("PostOpinion");
            _context.Remove(opinion);
            await _context.SaveChangesAsync();
        }

        public async Task SetHiddenPostOpinionAsync(Guid id, bool value)
        {
            var opinion = await _context.PostOpinions.FindAsync(id) ?? throw new EntityNotFoundException("PostOpinion");
            opinion.IsHidden = value;
            await _context.SaveChangesAsync();
        }

        public async Task<PostOpinion> UpdatePostOpinionAsync(Guid id, string content, int rate)
        {
            var opinion = await _context.PostOpinions.FindAsync(id) ?? throw new EntityNotFoundException("PostOpinion");
            opinion.Content = content;
            opinion.Rate = rate;
            opinion.LastUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return opinion;
        }
    }
}
