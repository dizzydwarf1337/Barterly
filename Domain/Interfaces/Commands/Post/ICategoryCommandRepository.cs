using Domain.Entities.Categories;

namespace Domain.Interfaces.Commands.Post;

public interface ICategoryCommandRepository
{
    Task<Category> CreateCategoryAsync(Category category, CancellationToken token);
    Task<Category> UpdateCategoryAsync(Category category, CancellationToken token);
    Task AddSubCategoryAsync(SubCategory subCategory, CancellationToken token);
    Task DeleteSubCategoryAsync(Guid id, CancellationToken token);
    Task DeleteCategoryAsync(Guid id, CancellationToken token);
}