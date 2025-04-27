using Domain.Entities.Categories;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post
{
    public class CategoryQueryRepository : BaseQueryRepository<BarterlyDbContext>, ICategoryQueryRepository
    {
        public CategoryQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.Include(x => x.SubCategories).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id) ?? throw new EntityNotFoundException("Category");
        }

        public async Task<ICollection<SubCategory>> GetSubCategoriesByCategory(Guid id)
        {
            return await _context.SubCategories.Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(Guid id)
        {
            return await _context.SubCategories.FindAsync(id) ?? throw new EntityNotFoundException("SubCategory");
        }
    }
}
