using Application.Core.ApiResponse;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Admins.Categories.EditCategory;

public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, ApiResponse<Category>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository;
    private readonly IMapper _mapper;

    public EditCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, IMapper mapper)
    {
        _categoryCommandRepository = categoryCommandRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<Category>> Handle(EditCategoryCommand request,
        CancellationToken cancellationToken)
    {
        return ApiResponse<Category>.Success(await UpdateAndSaveCategory(request, cancellationToken));
    }

    private async Task<Category> UpdateAndSaveCategory(EditCategoryCommand category, CancellationToken token)
    {
        var categoryToEdit = await _categoryCommandRepository.UpdateCategoryAsync(
            new Category
            {
                Id = category.Id,
                Description = category.Description,
                NameEN = category.NameEN,
                NamePL = category.NamePL,
                SubCategories = _mapper.Map<List<SubCategory>>(category.SubCategories)
            }, token);
        return categoryToEdit;
    }
}