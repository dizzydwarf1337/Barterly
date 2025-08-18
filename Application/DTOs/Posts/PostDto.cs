using System.ComponentModel.DataAnnotations;
using Application.DTOs.Categories;
using Domain.Enums.Posts;

namespace Application.DTOs.Posts;

public class PostDto
{
    public required string Id { get; set; }
    public required string OwnerId { get; set; }
    
    public required string Title { get; set; }

    public required string SubCategoryId { get; set; }
    
    public required string FullDescription { get; set; }
    
    public required string ShortDescription { get; set; }

    public decimal? Price { get; set; }

    public PostPriceType? PriceType { get; set; }

    public DateTime CreatedAt { get; set; }
    public PostPromotionType PromotionType { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? MainImageUrl { get; set; }
    public string[]? Tags { get; set; }

    public string? City { get; set; }
    public PostCurrency? Currency { get; set; }

    public string? Region { get; set; }
    public int? ViewsCount { get; set; }
    public string? Country { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public RentObjectType? RentObjectType { get; set; }
    public int? NumberOfRooms { get; set; }
    public decimal? Area { get; set; }

    public int? Floor { get; set; }
    public WorkloadType? Workload { get; set; }
    public WorkLocationType? WorkLocation { get; set; }
    public ContractType? Contract { get; set; }

    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? BuildingNumber { get; set; }
    public bool? ExperienceRequired { get; set; }
    public SubCategoryDto? SubCategory { get; set; }
    public ICollection<PostImageDto>? PostImages { get; set; }
}