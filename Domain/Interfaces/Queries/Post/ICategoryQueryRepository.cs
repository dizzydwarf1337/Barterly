using Domain.Entities.Categories;

namespace Domain.Interfaces.Queries.Post
{
    public interface ICategoryQueryRepository
    {
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<ICollection<Category>> GetCategoriesAsync();
        Task<ICollection<SubCategory>> GetSubCategoriesByCategory(Guid id);
        Task<SubCategory> GetSubCategoryByIdAsync(Guid id);
    }
}
