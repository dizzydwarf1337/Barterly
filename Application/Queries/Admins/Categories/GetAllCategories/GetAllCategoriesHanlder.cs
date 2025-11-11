using System.Linq.Expressions;
using Application.Core.ApiResponse;
using Domain.Entities.Categories;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Queries.Admins.Categories.GetAllCategories;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, ApiResponse<GetAllCategoriesQuery.Result>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;

    public GetAllCategoriesHandler(ICategoryQueryRepository categoryQueryRepository)
    {
        _categoryQueryRepository = categoryQueryRepository;
    }

    public async Task<ApiResponse<GetAllCategoriesQuery.Result>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.FilterBy?.PageSize <= 0 || request.FilterBy?.PageNumber <= 0)
                return ApiResponse<GetAllCategoriesQuery.Result>.Failure("Invalid pagination parameters.");
            
            var categoriesQuery =  _categoryQueryRepository.GetCategoriesAsync();
            
            if (request.FilterBy != null)
            {
                foreach (var filter in GetFilters(request.FilterBy))
                {
                    categoriesQuery = categoriesQuery.Where(filter);
                }
            }
            
            var totalCount = categoriesQuery.Count();
            
            if (request.SortBy != null && !string.IsNullOrWhiteSpace(request.SortBy.SortField))
            {
                categoriesQuery = ApplySorting(categoriesQuery, request.SortBy);
            }
            else
            {
                categoriesQuery = categoriesQuery.OrderByDescending(c => c.NameEN);
            }
            
            var pageNumber = request.FilterBy?.PageNumber ?? 1;
            var pageSize = request.FilterBy?.PageSize ?? 10;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var categories = categoriesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map to DTOs
            var categoryDtos = categories.Select(category => new GetAllCategoriesQuery.CategoryDto(
                Id: category.Id,
                NameEN: category.NameEN,
                NamePL: category.NamePL,
                Description: category.Description ?? "",
                SubCategoriesCount: category.SubCategories?.Count() ?? 0,
                SubCategories: category.SubCategories?.Select(sub => new GetAllCategoriesQuery.SubCategoryDto(
                    Id: sub.Id,
                    NameEN: sub.TitleEN,
                    NamePL: sub.TitlePL
                )) ?? Enumerable.Empty<GetAllCategoriesQuery.SubCategoryDto>()
            ));

            var result = new GetAllCategoriesQuery.Result(
                Categories: categoryDtos,
                TotalCount: totalCount,
                TotalPages: totalPages,
                CurrentPage: pageNumber,
                PageSize: pageSize
            );

            return ApiResponse<GetAllCategoriesQuery.Result>.Success(result);
        }
        catch (Exception ex)
        {
            return ApiResponse<GetAllCategoriesQuery.Result>.Failure($"Error retrieving categories: {ex.Message}");
        }
    }

    private static IEnumerable<Expression<Func<Category, bool>>> GetFilters(FilterBy filterBy)
    {
        if (!string.IsNullOrWhiteSpace(filterBy.Search))
        {
            var searchTerm = filterBy.Search.ToLower();
            yield return category => 
                category.NamePL.ToLower().Contains(searchTerm) ||
                category.NameEN.ToLower().Contains(searchTerm) ||
                (category.Description != null && category.Description.ToLower().Contains(searchTerm));
        }
        
        if (filterBy.HasSubCategories.HasValue)
        {
            if (filterBy.HasSubCategories.Value)
            {
                yield return category => category.SubCategories != null && category.SubCategories.Any();
            }
            else
            {
                yield return category => category.SubCategories == null || !category.SubCategories.Any();
            }
        }
    }

    private static IQueryable<Category> ApplySorting(IQueryable<Category> query, SortBy sortBy)
    {
        var sortField = sortBy.SortField?.ToLower();
        var isDescending = sortBy.IsDescending;

        return sortField switch
        {
            "namepl" => isDescending 
                ? query.OrderByDescending(c => c.NamePL) 
                : query.OrderBy(c => c.NamePL),
            
            "nameen" => isDescending 
                ? query.OrderByDescending(c => c.NameEN) 
                : query.OrderBy(c => c.NameEN),
            
            "description" => isDescending 
                ? query.OrderByDescending(c => c.Description) 
                : query.OrderBy(c => c.Description),
            
            "subcategoriescount" => isDescending 
                ? query.OrderByDescending(c => c.SubCategories != null ? c.SubCategories.Count() : 0) 
                : query.OrderBy(c => c.SubCategories != null ? c.SubCategories.Count() : 0),
            
            _ => isDescending 
                ? query.OrderByDescending(c => c.NameEN) 
                : query.OrderBy(c => c.NameEN)
        };
    }
}