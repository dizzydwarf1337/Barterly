using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class FavouriteCategoryQueryRepository : BaseQueryRepository<BarterlyDbContext>,
    IFavouriteCategoryQueryRepository
{
    public FavouriteCategoryQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesAsync(CancellationToken token)
    {
        return await _context.FavouriteCategories.ToListAsync(token);
    }

    public async Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesByUserIdAsync(Guid userId,
        CancellationToken token)
    {
        return await _context.FavouriteCategories.Where(x => x.UserId == userId).ToListAsync(token);
    }

    public async Task<FavouriteCategory> GetFavouriteCategoryByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.FavouriteCategories.FindAsync(id, token) ??
               throw new EntityNotFoundException("Favourite Categories");
    }
}