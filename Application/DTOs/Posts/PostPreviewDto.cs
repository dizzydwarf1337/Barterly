using Domain.Enums.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class PostPreviewDto
    {
        public string SubCategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string? ShortDescription { get; set; }
        public decimal? Price { get; set; }
        public PostPriceType PriceType { get; set; }
        public string? MainImageUrl { get; set; }
        public PostPromotionType PostPromotionType { get; set; }
    }
}
