using Application.Core.ApiResponse;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Admins.Categories.AddCategory;

public class AddCategoryCommandHanlder : IRequestHandler<AddCategoryCommand, ApiResponse<Unit>>
{
    private readonly ICategoryCommandRepository _categoryCommandRepository;
    private readonly ILogService _logService;
    private readonly IMapper _mapper;

    public AddCategoryCommandHanlder(ICategoryCommandRepository categoryCommandRepository, ILogService logService,
        IMapper mapper)
    {
        _categoryCommandRepository = categoryCommandRepository;
        _logService = logService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<Unit>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToAdd = new Category()
        {
            NameEN = request.NameEN,
            NamePL = request.NamePL,
            Description = request.Description
        };

        categoryToAdd = AddDefaultSubCategory(categoryToAdd);
        if (request.SubCategories.Count > 0)
        {
            foreach (var x in request.SubCategories)
            {
                categoryToAdd.SubCategories.Add(new SubCategory
                {
                    CategoryId = categoryToAdd.Id,
                    TitleEN = x.NameEn,
                    TitlePL = x.NamePl
                });
            }
        }
        await SaveCategory(categoryToAdd, cancellationToken);

        return ApiResponse<Unit>.Success(Unit.Value, 201);
    }

    private Category AddDefaultSubCategory(Category category)
    {
        var subCategory = new SubCategory
        {
            CategoryId = category.Id,
            TitleEN = "All",
            TitlePL = "Wszystkie"
        };
        category.SubCategories.Add(subCategory);
        return category;
    }

    private async Task SaveCategory(Category category, CancellationToken token)
    {
        await _categoryCommandRepository.CreateCategoryAsync(category, token);
        await _logService.CreateLogAsync($"Category created id: {category.Id} name: {category.NameEN}", token,
            LogType.Information);
    }
}