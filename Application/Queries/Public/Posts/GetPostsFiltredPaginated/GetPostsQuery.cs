using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Domain.Enums.Posts;

namespace Application.Queries.Public.Posts.GetPostsFiltredPaginated;

public class GetPostsQuery : PublicRequest<GetPostsQuery.Result>
{
    public FilterSpecification? FilterBy { get; set; }
    public SortSpecification? SortBy { get; set; }

    public class FilterSpecification
    {
        public string? Search { get; set; }
        
        public Guid? CategoryId { get; set; }
        
        public Guid? SubCategoryId { get; set; }
        
        public string? City { get; set; }
        
        public decimal? MinPrice { get; set; }
        
        public decimal? MaxPrice { get; set; }
        
        public string? PostType { get; set; }
        
        public RentObjectType? RentObjectType { get; set; }
        
        public int? NumberOfRooms { get; set; }
        
        public decimal? Area { get; set; }
        
        public int? Floor { get; set; }
        
        public WorkloadType? Workload { get; set; }
        
        public WorkLocationType? WorkLocation { get; set; }
        
        public decimal? MinSalary { get; set; }
        
        public decimal? MaxSalary { get; set; }
        
        public bool? ExperienceRequired { get; set; }
        
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
        public List<PostPreviewDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}