using Application.DTOs.Categories;
using Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategory(CategoryDto category);
        Task DeleteCategory(Guid categoryId);
        Task AddSubCategory(SubCategoryDto subCategoryDto);
        Task DeleteSubCategory(Guid subCategoryId);
        Task<ICollection<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategory(Guid categoryId);
        Task<SubCategoryDto> GetSubCategory(Guid subCategoryId);
        Task<ICollection<SubCategoryDto>> GetSubCategoriesByCategoryId(Guid categoryId);
        Task<Category?> GetCategoryByName(string name);
    }
}
