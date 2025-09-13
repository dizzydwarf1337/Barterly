using Application.Core.ApiResponse;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Admins.Categories.EditCategory;

public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, ApiResponse<Unit>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository;
    private readonly IMapper _mapper;

    public EditCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, IMapper mapper)
    {
        _categoryCommandRepository = categoryCommandRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<Unit>> Handle(EditCategoryCommand request,
        CancellationToken cancellationToken)
    {
        return ApiResponse<Unit>.Success(await UpdateAndSaveCategory(request, cancellationToken));
    }

    private async Task<Unit> UpdateAndSaveCategory(EditCategoryCommand category, CancellationToken token)
    {
        var categoryToEdit = await _categoryCommandRepository.UpdateCategoryAsync(
            new Category
            {
                Id = category.Id,
                Description = category.Description,
                NameEN = category.NameEn,
                NamePL = category.NamePl,
                SubCategories = _mapper.Map<List<SubCategory>>(category.SubCategories)
            }, token);
        return Unit.Value;
    }
}