using Domain.Entities;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Users
{
    public class FavouriteCategoryQueryRepository : BaseQueryRepository<BarterlyDbContext>, IFavouriteCategoryQueryRepository
    {
        public FavouriteCategoryQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesAsync()
        {
            return await _context.FavouriteCategories.ToListAsync();
        }

        public async Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesByUserIdAsync(Guid userId)
        {
            return await _context.FavouriteCategories.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<FavouriteCategory> GetFavouriteCategoryByIdAsync(Guid id)
        {
            return await _context.FavouriteCategories.FindAsync(id) ?? throw new Exception("Favourite Categories not found");
        }
    }
}
