using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Post
{
    public class CategoryCommandRepository : BaseCommandRepository<BarterlyDbContext>, ICategoryCommandRepository
    {
        public CategoryCommandRepository(BarterlyDbContext context) : base(context)
        {
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

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
