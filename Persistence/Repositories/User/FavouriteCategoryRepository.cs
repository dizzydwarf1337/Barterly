using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class FavouriteCategoryRepository : BaseRepository, IFavouriteCategoryCommandRepository, IFavouriteCategoryQueryRepository
    {
        public FavouriteCategoryRepository(BarterlyDbContext context) : base(context) { }

        public async Task CreateFavouriteCategoryAsync(FavouriteCategory favouriteCategory)
        {
            await _context.FavouriteCategories.AddAsync(favouriteCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFavouriteCategoryAsync(Guid id)
        {
            var favouriteCategory = await GetFavouriteCategoryByIdAsync(id);
            _context.FavouriteCategories.Remove(favouriteCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesAsync()
        {
            return await _context.FavouriteCategories.ToListAsync();
        }

        public async Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesByUserIdAsync(Guid userId)
        {
            return await _context.FavouriteCategories.Where(x=>x.UserId==userId).ToListAsync();
        }

        public async Task<FavouriteCategory> GetFavouriteCategoryByIdAsync(Guid id)
        {
            return await _context.FavouriteCategories.FindAsync(id) ?? throw new Exception("Favourite Categories not found");
        }
    }
}
