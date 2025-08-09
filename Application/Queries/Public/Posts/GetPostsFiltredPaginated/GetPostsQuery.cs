using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Posts.GetPostsFiltredPaginated;

public class GetPostsQuery : PublicRequest<GetPostsQuery.Result>
{
    public FilterSpecification? FilterBy { get; set; }
    public SortSpecification? SortBy { get; set; }

    public class FilterSpecification
    {
        public string? Search { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid? UserId { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }

    public class SortSpecification
    {
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }

    public class Result
    {
        public List<PostPreviewDto> Posts { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}