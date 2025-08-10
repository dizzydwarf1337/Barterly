using Domain.Entities.Categories;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post;

public class CategoryQueryRepository : BaseQueryRepository<BarterlyDbContext>, ICategoryQueryRepository
{
    public CategoryQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<ICollection<Category>> GetCategoriesAsync(CancellationToken token)
    {
        return await _context.Categories.Include(x => x.SubCategories).ToListAsync(token);
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Categories.FindAsync(id, token) ?? throw new EntityNotFoundException("Category");
    }

    public async Task<ICollection<SubCategory>> GetSubCategoriesByCategory(Guid id, CancellationToken token)
    {
        return await _context.SubCategories.Where(x => x.CategoryId == id).ToListAsync(token);
    }

    public async Task<SubCategory> GetSubCategoryByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.SubCategories.FindAsync(id, token) ??
               throw new EntityNotFoundException("SubCategory");
    }
}