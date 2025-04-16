using Domain.Entities.Categories;

namespace Domain.Interfaces.Commands.Post
{
    public interface ICategoryCommandRepository
    {
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task AddSubCategoryAsync(SubCategory subCategory);
        Task DeleteSubCategoryAsync(Guid id);
        Task DeleteCategoryAsync(Guid id);
    }
}
