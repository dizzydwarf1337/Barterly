using API.Core.ApiResponse;
using Application.DTOs.Categories;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, ApiResponse<ICollection<CategoryDto>>>
    {
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly IMapper _mapper;
        public GetAllCategoriesHandler(ICategoryQueryRepository categoryQueryRepository,IMapper mapper)
        {
            _categoryQueryRepository = categoryQueryRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ICollection<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {

            var categories = await _categoryQueryRepository.GetCategoriesAsync();
            return ApiResponse<ICollection<CategoryDto>>.Success(_mapper.Map<ICollection<CategoryDto>>(categories));


        }
    }
}
