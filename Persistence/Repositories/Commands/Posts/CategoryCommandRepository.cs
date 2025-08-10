using Domain.Entities.Categories;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post;

public class CategoryCommandRepository : BaseCommandRepository<BarterlyDbContext>, ICategoryCommandRepository
{
    public CategoryCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task AddSubCategoryAsync(SubCategory subCategory, CancellationToken token)
    {
        await _context.SubCategories.AddAsync(subCategory, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken token)
    {
        await _context.Categories.AddAsync(category, token);
        await _context.SaveChangesAsync(token);
        return category;
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken token)
    {
        var category = await _context.Categories.FindAsync(id) ?? throw new EntityNotFoundException("Category");
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteSubCategoryAsync(Guid id, CancellationToken token)
    {
        await _context.SubCategories.Where(x => x.Id == id).ExecuteDeleteAsync(token);
        await _context.SaveChangesAsync(token);
    }

    public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken token)
    {
        var categoryOld = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == category.Id) ?? throw new EntityNotFoundException("Category");

        if (categoryOld.SubCategories != null) _context.SubCategories.RemoveRange(categoryOld.SubCategories);

        if (category.SubCategories != null)
        {
            foreach (var sub in category.SubCategories) sub.CategoryId = category.Id;

            await _context.SubCategories.AddRangeAsync(category.SubCategories.Where(s =>
                !string.IsNullOrWhiteSpace(s.TitlePL) && !string.IsNullOrWhiteSpace(s.TitleEN)));
        }

        categoryOld.NameEN = category.NameEN;
        categoryOld.NamePL = category.NamePL;
        categoryOld.Description = category.Description;

        await _context.SaveChangesAsync(token);
        return category;
    }
}