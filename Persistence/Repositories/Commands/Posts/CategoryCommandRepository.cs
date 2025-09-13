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
        var category = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id, token) ?? throw new EntityNotFoundException("Category");
        var postExists = await _context.Posts.Include(x=>x.SubCategory).AnyAsync(x => x.SubCategory.CategoryId == category.Id, token);
        if (postExists)
            throw new ApplicationException("Cannot remove category with provided posts");
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
                              .FirstOrDefaultAsync(c => c.Id == category.Id, token)
                          ?? throw new EntityNotFoundException("Category");
        
        categoryOld.NameEN = category.NameEN;
        categoryOld.NamePL = category.NamePL;
        categoryOld.Description = category.Description;
        
        categoryOld.SubCategories = category.SubCategories;

        await _context.SaveChangesAsync(token);
        return categoryOld;
    }
}