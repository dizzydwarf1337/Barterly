using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommandHanlder : IRequestHandler<AddCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryService _categoryService;

        public AddCategoryCommandHanlder(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ApiResponse<Unit>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.AddCategory(request.category);
                return ApiResponse<Unit>.Success(Unit.Value, 201);
            }
            catch (OperationCanceledException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 409);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message);
            }
        }
    }
}
