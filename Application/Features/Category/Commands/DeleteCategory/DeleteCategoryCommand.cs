using API.Core.ApiResponse;
using MediatR;

namespace Application.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<ApiResponse<Unit>>
    {
        public string CategoryId { get; set; }
    }
}
