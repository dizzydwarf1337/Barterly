using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post;

public class PromotionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPromotionQueryRepository
{
    public PromotionQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<Promotion> GetPromotionByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Promotions.FindAsync(id, token) ??
               throw new EntityNotFoundException("Post Promotion");
    }

    public async Task<Promotion> GetPromotionByPostIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Promotions.FirstOrDefaultAsync(x => x.PostId == id, token) ??
               throw new EntityNotFoundException("Post Promotion");
    }
}