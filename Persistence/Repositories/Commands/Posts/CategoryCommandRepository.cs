using Domain.Entities.Categories;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post
{
    public class CategoryCommandRepository : BaseCommandRepository<BarterlyDbContext>, ICategoryCommandRepository
    {
        public CategoryCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task AddSubCategoryAsync(SubCategory subCategory)
        {
            await _context.SubCategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCategoryAsync(Category category)
        {

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id) ?? throw new Exception("Category not found");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubCategoryAsync(Guid id)
        {
            await _context.SubCategories.Where(x => x.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var categoryOld = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (categoryOld == null) return;
            _context.SubCategories.RemoveRange(categoryOld.SubCategories);
            foreach (var sub in category.SubCategories)
            {
                sub.CategoryId = category.Id; 
            }

            await _context.SubCategories.AddRangeAsync(category.SubCategories.Where(s=>!String.IsNullOrWhiteSpace(s.TitlePL) && !String.IsNullOrWhiteSpace(s.TitleEN)));
            categoryOld.NameEN = category.NameEN;
            categoryOld.NamePL = category.NamePL;
            categoryOld.Description = category.Description;

            await _context.SaveChangesAsync();
        }
    }
}
