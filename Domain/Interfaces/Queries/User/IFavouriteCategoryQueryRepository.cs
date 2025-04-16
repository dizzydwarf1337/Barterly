using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IFavouriteCategoryQueryRepository
    {
        Task<FavouriteCategory> GetFavouriteCategoryByIdAsync(Guid id);
        Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesAsync();
        Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesByUserIdAsync(Guid userId);
    }
}
