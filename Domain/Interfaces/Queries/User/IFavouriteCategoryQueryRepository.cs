using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface IFavouriteCategoryQueryRepository
{
    Task<FavouriteCategory> GetFavouriteCategoryByIdAsync(Guid id, CancellationToken token);
    Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesAsync(CancellationToken token);
    Task<ICollection<FavouriteCategory>> GetFavouriteCategoriesByUserIdAsync(Guid userId, CancellationToken token);
}