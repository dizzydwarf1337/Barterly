using Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
