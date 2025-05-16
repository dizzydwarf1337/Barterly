using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;

namespace Application.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogService _logService;

        public DeleteCategoryCommandHandler(ICategoryService categoryService,ILogService logService)
        {
            _categoryService = categoryService;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.DeleteCategory(Guid.Parse(request.CategoryId));
                await _logService.CreateLogAsync($"Category deleted id: {request.CategoryId}",LogType.Information);
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message);
            }
        }
    }
}
