using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Categories;
using Domain.Entities.Users;
using Domain.Enums.Posts;

namespace Domain.Entities.Posts;

public class Post
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SubCategoryId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid PromotionId { get; set; }
    public Guid PostSettingsId { get; set; }

    [Required] [MaxLength(50)] public required string Title { get; set; }

    [Required] [MaxLength(500)] public required string FullDescription { get; set; }

    [MaxLength(150)] public string? ShortDescription { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal? Price { get; set; }

    public PostPriceType? PriceType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = null;

    public int ViewsCount { get; set; } = 0;
    public string? MainImageUrl { get; set; }
    public string[] Tags { get; set; } = [];
    public PostCurrency Currency { get; set; }

    [MaxLength(100)] public string? City { get; set; }

    [MaxLength(100)] public string? Region { get; set; }
    [MaxLength(100)] public string? Country { get; set; }

    [MaxLength(100)] public string? Street { get; set; }
    [MaxLength(100)] public string? HouseNumber { get; set; }

    [ForeignKey("SubCategoryId")] public virtual SubCategory SubCategory { get; set; } = default!;
    [ForeignKey("OwnerId")] public virtual User Owner { get; set; } = default!;

    [ForeignKey("PromotionId")] public virtual Promotion Promotion { get; set; } = default!;
    [ForeignKey("PostSettingsId")] public virtual PostSettings PostSettings { get; set; } = default!;

    public virtual ICollection<PostImage>? PostImages { get; set; } = new List<PostImage>();
    public virtual ICollection<PostOpinion>? PostOpinions { get; set; } = new List<PostOpinion>();
    public virtual ICollection<ReportPost>? PostReports { get; set; } = new List<ReportPost>();
    public virtual ICollection<VisitedPost>? VisitedPosts { get; set; } = new List<VisitedPost>();

    public override bool Equals(object? obj)
    {
        return obj is Post post &&
               Id.Equals(post.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}