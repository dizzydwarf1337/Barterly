using Domain.Entities;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Post
{
    public class CategoryQueryRepository : BaseQueryRepository<BarterlyDbContext>, ICategoryQueryRepository
    {
        public CategoryQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.Include(x=>x.SubCategories).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id) ?? throw new Exception("Category not found");
        }

        public async Task<ICollection<SubCategory>> GetSubCategoriesByCategory(Guid id)
        {
            return await _context.SubCategories.Where(x=>x.CategoryId==id).ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(Guid id)
        {
            return await _context.SubCategories.FindAsync(id);
        }
    }
}
