using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface IFavouriteCategoryCommandRepository
    {
        Task CreateFavouriteCategoryAsync(FavouriteCategory favouriteCategory);
        Task DeleteFavouriteCategoryAsync(Guid id);
    }
}
