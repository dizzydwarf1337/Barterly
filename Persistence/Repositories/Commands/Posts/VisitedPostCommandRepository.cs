using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post
{
    public class VisitedPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IVisitedPostCommandRepository
    {
        public VisitedPostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateVisitedPostAsync(VisitedPost visitedPost)
        {
            await _context.VisitedPosts.AddAsync(visitedPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitedPostAsync(Guid id)
        {
            var visitedPost = await _context.VisitedPosts.FindAsync(id) ?? throw new EntityNotFoundException("VisitedPost");
            _context.VisitedPosts.Remove(visitedPost);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateVisitedPost(VisitedPost post)
        {
            _context.VisitedPosts.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}
