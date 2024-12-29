using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeletedPost
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SubCategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid PromotionId { get; set; }

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

    }
}
