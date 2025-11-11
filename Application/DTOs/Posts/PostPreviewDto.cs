using Domain.Enums.Posts;

namespace Application.DTOs.Posts;

public class PostPreviewDto
{
    public required string Id { get; set; }
    public required string SubCategoryId { get; set; }
    public Guid OwnerId { get; set; }
    public string? Title { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public string? ShortDescription { get; set; }
    public decimal? Price { get; set; }

    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public PostPriceType PriceType { get; set; }
    public string? MainImageUrl { get; set; }
    public PostPromotionType? PostPromotionType { get; set; }
    public WorkloadType? Workload { get; set; }
    public WorkLocationType? WorkLocation { get; set; }
    public ContractType? Contract { get; set; }
    public bool? ExperienceRequired { get; set; }
    public RentObjectType RentObjectType { get; set; }
    public PostCurrency? Currency { get; set; }
    public int? NumberOfRooms { get; set; }
    public decimal? Area { get; set; }
    public int? ViewsCount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? Floor { get; set; }
    public string? PostType { get; set; }
    public string? OwnerName { get; set; }
}