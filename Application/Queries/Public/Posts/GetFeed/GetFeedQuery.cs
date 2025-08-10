using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Posts.GetFeed;

public class GetFeedQuery : PublicRequest<GetFeedQuery.Result>
{
    public FilterSpecification? FilterBy { get; set; }
    public SortSpecification? SortBy { get; set; }

    public class FilterSpecification
    {
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
        public ICollection<PostPreviewDto> Posts { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}