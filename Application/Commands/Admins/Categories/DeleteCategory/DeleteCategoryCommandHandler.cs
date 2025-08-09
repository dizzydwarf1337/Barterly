using Application.Core.ApiResponse;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Admins.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<Unit>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository;
    private readonly ILogService _logService;

    public DeleteCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository,
        ILogService logService)
    {
        _categoryCommandRepository = categoryCommandRepository;
        _logService = logService;
    }

    public async Task<ApiResponse<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _categoryCommandRepository.DeleteCategoryAsync(request.CategoryId, cancellationToken);
        await _logService.CreateLogAsync($"Category deleted id: {request.CategoryId}", cancellationToken,
            LogType.Information);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}