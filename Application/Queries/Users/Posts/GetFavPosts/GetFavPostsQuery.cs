using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetFavPosts;

public class GetFavPostsQuery : UserRequest<GetFavPostsQuery.Result>
{
    public FilterSpecification? FilterBy { get; set; }

    public class FilterSpecification
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
    
    public class Result
    {
        public List<PostPreviewDto> Items { get; set; }
        public int TotalCount { get; set; }
        public double TotalPages { get; set; }
    }
}