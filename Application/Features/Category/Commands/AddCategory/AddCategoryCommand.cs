using API.Core.ApiResponse;
using Application.DTOs.Categories;
using MediatR;

namespace Application.Features.Category.Commands.AddCategory
{

    public class AddCategoryCommand : IRequest<ApiResponse<Unit>>
    {
        public required CategoryDto category;
    }

}
