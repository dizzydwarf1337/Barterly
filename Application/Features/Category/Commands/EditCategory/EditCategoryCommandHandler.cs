using API.Core.ApiResponse;
using Application.Interfaces;
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

        public EditCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ApiResponse<Unit>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.EditCategory(request.category);
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
