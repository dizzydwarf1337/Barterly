using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface IFavouriteCategoryQueryRepository
    {
        Task<FavouriteCategory> GetFavouriteCategoryByIdAsync(Guid id);
        Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesAsync();
        Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesByUserIdAsync(Guid userId);
    }
}
