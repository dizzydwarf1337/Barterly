using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryCommandRepository categoryCommandRepository, ICategoryQueryRepository categoryQueryRepository, IMapper mapper)
        {
            _categoryCommandRepository = categoryCommandRepository;
            _categoryQueryRepository = categoryQueryRepository;
            _mapper = mapper;
        }

        public async Task AddCategory(CategoryDto category)
        {
            var categoryToAdd = _mapper.Map<Category>(category);
            await _categoryCommandRepository.CreateCategoryAsync(categoryToAdd);
        }

        public async Task AddSubCategory(SubCategoryDto subCategoryDto)
        {
            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);
            await _categoryCommandRepository.AddSubCategoryAsync(subCategory);
        }

        public async Task DeleteCategory(Guid categoryId)
        {
            await _categoryCommandRepository.DeleteCategoryAsync(categoryId);
        }

        public async Task DeleteSubCategory(Guid subCategoryId)
        {
            await _categoryCommandRepository.DeleteSubCategoryAsync(subCategoryId);
        }

        public async Task<ICollection<CategoryDto>> GetAllCategories()
        {
            var categories =  await _categoryQueryRepository.GetCategoriesAsync();
            return _mapper.Map<ICollection<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategory(Guid categoryId)
        {
            var category = await _categoryQueryRepository.GetCategoryByIdAsync(categoryId);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<Category?> GetCategoryByName(string name)
        {
            var categories = await _categoryQueryRepository.GetCategoriesAsync();
           
            return  categories.FirstOrDefault(x => String.Equals(x.NameEN.ToLower(),name.ToLower()) || String.Equals(x.NamePL.ToLower(),name.ToLower()));
            
        }

        public async Task<ICollection<SubCategoryDto>> GetSubCategoriesByCategoryId(Guid categoryId)
        {
            var subCategories = await _categoryQueryRepository.GetSubCategoriesByCategory(categoryId);
            return _mapper.Map<ICollection<SubCategoryDto>>(subCategories);
        }

        public async Task<SubCategoryDto> GetSubCategory(Guid subCategoryId)
        {
            var subCategory = await _categoryQueryRepository.GetSubCategoryByIdAsync(subCategoryId);
            return _mapper.Map<SubCategoryDto>(subCategory);
        }
    }
}
