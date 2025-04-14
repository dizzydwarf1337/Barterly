using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommandHanlder : IRequestHandler<AddCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryService _categoryService;

        public AddCategoryCommandHanlder(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        async Task<ApiResponse<Unit>> IRequestHandler<AddCategoryCommand, ApiResponse<Unit>>.Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.AddCategory(request.category);
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (Exception ex) {
                return ApiResponse<Unit>.Failure(ex.Message);
            }
        }
    }
}
