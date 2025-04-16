
using Domain.Entities.Categories;
using Domain.Entities.Users;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Posts
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SubCategoryId { get; set; }

        public Guid OwnerId { get; set; }

        public Guid PromotionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Region { get; set; }

        [Required]
        [MaxLength(500)]
        public string FullDescription { get; set; }

        [MaxLength(150)]
        public string? ShortDescription { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public PostPriceType PriceType { get; set; } = PostPriceType.OnetimePayment;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = null;

        public bool IsHidden { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public string? MainImageUrl { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        [ForeignKey("PromotionId")]
        public virtual Promotion Promotion { get; set; }

        public virtual ICollection<PostImage>? PostImages { get; set; }
        public virtual ICollection<PostOpinion>? PostOpinions { get; set; }
        public virtual ICollection<ReportPost>? PostReports { get; set; }
        public virtual ICollection<VisitedPost>? VisitedPosts { get; set; }
    }
}
