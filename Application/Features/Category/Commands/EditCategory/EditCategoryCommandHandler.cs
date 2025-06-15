using API.Core.ApiResponse;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
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
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public EditCategoryCommandHandler(ICategoryCommandRepository categoryCommandRepository, IMapper mapper,ILogService logService)
        {
            _categoryCommandRepository = categoryCommandRepository;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToEdit = _mapper.Map<Domain.Entities.Categories.Category>(request.category);
            await _categoryCommandRepository.UpdateCategoryAsync(categoryToEdit);
            await _logService.CreateLogAsync($"Category edited id: {request.category.Id} name: {request.category.NameEN}", LogType.Information);
            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
