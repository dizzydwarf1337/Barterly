using API.Core.ApiResponse;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommandHanlder : IRequestHandler<AddCategoryCommand, ApiResponse<Unit>>
    {
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public AddCategoryCommandHanlder(ICategoryCommandRepository categoryCommandRepository, ILogService logService, IMapper mapper)
        {
            _categoryCommandRepository = categoryCommandRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Unit>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToAdd = _mapper.Map<Domain.Entities.Categories.Category>(request.category);
            categoryToAdd.SubCategories = [.. categoryToAdd.SubCategories.Where(x => !string.IsNullOrWhiteSpace(x.TitlePL) && !string.IsNullOrWhiteSpace(x.TitleEN))];
            categoryToAdd.SubCategories.Add(new SubCategory
            {
                Id = new Guid(),
                CategoryId = categoryToAdd.Id,
                TitleEN = "All",
                TitlePL = "Wszystkie",
            });

            await _categoryCommandRepository.CreateCategoryAsync(categoryToAdd);
            await _logService.CreateLogAsync($"Category created id: {request.category.Id} name: {request.category.NameEN}",LogType.Information);
            return ApiResponse<Unit>.Success(Unit.Value, 201);
        }
    }
}
