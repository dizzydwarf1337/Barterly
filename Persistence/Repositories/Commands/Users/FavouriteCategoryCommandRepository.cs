using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class FavouriteCategoryCommandRepository : BaseCommandRepository<BarterlyDbContext>, IFavouriteCategoryCommandRepository
    {
        public FavouriteCategoryCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateFavouriteCategoryAsync(FavouriteCategory favouriteCategory)
        {
            await _context.FavouriteCategories.AddAsync(favouriteCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFavouriteCategoryAsync(Guid id)
        {
            var favouriteCategory = await _context.FavouriteCategories.FindAsync(id) ?? throw new EntityNotFoundException("Favourite category");
            _context.FavouriteCategories.Remove(favouriteCategory);
            await _context.SaveChangesAsync();
        }
    }
}
