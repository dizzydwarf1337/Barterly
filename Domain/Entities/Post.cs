using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string FullDescription { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public PostPriceType PriceType { get; set; } = PostPriceType.OnetimePayment;

        public string? ShortDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = null;

        public bool IsHidden { get; set; } = false;

        public string? MainImageUrl { get; set; }
        
        public Guid SubCategoryId { get; set; }
        
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        
        public Guid OwnerId { get; set; }
        
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        public Guid PromotionId { get; set; }

        [ForeignKey("PromotionId")]
        public virtual Promotion Promotion { get; set; }
        
        public virtual ICollection<PostImage>? PostImages { get; set; }
        public virtual ICollection<PostOpinion>? PostOpinsions { get; set; }
        public virtual ICollection<ReportPost>? PostReports { get; set; }
    }
}
