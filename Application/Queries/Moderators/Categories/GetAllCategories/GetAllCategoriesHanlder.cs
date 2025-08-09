using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Queries.Moderators.Categories.GetAllCategories;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery,
    ApiResponse<IEnumerable<GetAllCategoriesQuery.Result>>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;

    public GetAllCategoriesHandler(ICategoryQueryRepository categoryQueryRepository)
    {
        _categoryQueryRepository = categoryQueryRepository;
    }

    public async Task<ApiResponse<IEnumerable<GetAllCategoriesQuery.Result>>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryQueryRepository.GetCategoriesAsync(cancellationToken);
        return ApiResponse<IEnumerable<GetAllCategoriesQuery.Result>>.Success(categories
            .Select(x =>
                new GetAllCategoriesQuery.Result(
                    x.Id,
                    x.NameEN,
                    x.NameEN,
                    x.Description ?? "",
                    x.SubCategories.Select(y => new GetAllCategoriesQuery.SubCategory(y.Id, y.TitleEN, y.TitlePL)
                    ).ToList())));
    }
}