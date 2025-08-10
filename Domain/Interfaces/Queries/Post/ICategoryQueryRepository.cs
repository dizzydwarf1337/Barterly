using Domain.Entities.Categories;

namespace Domain.Interfaces.Queries.Post;

public interface ICategoryQueryRepository
{
    Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken token);
    Task<ICollection<Category>> GetCategoriesAsync(CancellationToken token);
    Task<ICollection<SubCategory>> GetSubCategoriesByCategory(Guid id, CancellationToken token);
    Task<SubCategory> GetSubCategoryByIdAsync(Guid id, CancellationToken token);
}