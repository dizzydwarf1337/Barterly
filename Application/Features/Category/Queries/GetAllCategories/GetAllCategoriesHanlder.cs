using API.Core.ApiResponse;
using Application.DTOs.Categories;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Category.Queries.GetAllCategories
{
    public class Handler : IRequestHandler<GetAllCategoriesQuery, ApiResponse<ICollection<CategoryDto>>>
    {
        private readonly ICategoryService _categoryService;

        public Handler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ApiResponse<ICollection<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return ApiResponse<ICollection<CategoryDto>>.Success(await _categoryService.GetAllCategories());
            }
            catch
            {
                return ApiResponse<ICollection<CategoryDto>>.Failure("Error while loading categories");
            }
        }
    }
}
