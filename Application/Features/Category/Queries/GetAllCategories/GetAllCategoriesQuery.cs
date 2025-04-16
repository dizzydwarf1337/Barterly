using API.Core.ApiResponse;
using Application.DTOs.Categories;
using MediatR;

namespace Application.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<ApiResponse<ICollection<CategoryDto>>>
    {

    }

}

