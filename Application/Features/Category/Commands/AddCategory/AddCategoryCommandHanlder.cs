using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Enums.Common;
using MediatR;

namespace Application.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommandHanlder : IRequestHandler<AddCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogService _logService;

        public AddCategoryCommandHanlder(ICategoryService categoryService, ILogService logService)
        {
            _categoryService = categoryService;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.AddCategory(request.category);
                await _logService.CreateLogAsync($"Category created id: {request.category.Id} name: {request.category.NameEN}",LogType.Information);
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
