using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryService _categoryService;

        public DeleteCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ApiResponse<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.DeleteCategory(Guid.Parse(request.CategoryId));
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message);
            }
        }
    }
}
