using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface ICategoryCommandRepository
    {
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task AddSubCategoryAsync(SubCategory subCategory);
        Task DeleteSubCategoryAsync(Guid id);
        Task DeleteCategoryAsync(Guid id);
    }
}
