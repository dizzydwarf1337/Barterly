using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post
{
    public class VisitedPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IVisitedPostCommandRepository
    {
        public VisitedPostCommandRepository(BarterlyDbContext context) : base(context)
        {
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
        public async Task VisitPost (Guid postId, Guid userId)
        {
            var visitedPost = await _context.VisitedPosts.Where(x => x.UserId == userId && x.PostId == postId).FirstOrDefaultAsync();
            if (visitedPost != null)
            {
                visitedPost.VisitedCount++;
                _context.VisitedPosts.Update(visitedPost);
            }
            else
            {
                visitedPost = new VisitedPost
                {
                    PostId = postId,
                    UserId = userId,
                    VisitedCount = 0,
                };
                await _context.VisitedPosts.AddAsync(visitedPost);
            }
        }
    }
}
