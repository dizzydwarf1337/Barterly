using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Entities.Categories;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly ILogService _logService;

        public DeleteCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, ILogService logService)
        {
            _categoryCommandRepository = categoryCommandRepository;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryCommandRepository.DeleteCategoryAsync(Guid.Parse(request.CategoryId));
            await _logService.CreateLogAsync($"Category deleted id: {request.CategoryId}",LogType.Information);
            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
