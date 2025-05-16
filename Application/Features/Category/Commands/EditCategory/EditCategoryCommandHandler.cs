using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Commands.EditCategory
{
    public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogService _logService;

        public EditCategoryCommandHandler(ICategoryService categoryService, ILogService logService)
        {
            _categoryService = categoryService;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.EditCategory(request.category);
                await _logService.CreateLogAsync($"Category edited id: {request.category.Id} name: {request.category.NameEN}", LogType.Information);
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 404);
            }
            catch (OperationCanceledException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 409);
            }
            catch (Exception ex) {
                return ApiResponse<Unit>.Failure(ex.Message);
            }
        }
    }
}
