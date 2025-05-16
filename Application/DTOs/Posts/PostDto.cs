using Application.DTOs.Categories;
using Domain.Entities.Categories;
using Domain.Entities.Posts;
using Domain.Enums.Posts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Application.DTOs.Posts
{
    public class PostDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 50 characters long")]
        public string Title { get; set; }
        public string SubCategoryId { get; set; }
        public string PromotionId { get; set; }


        [Required(ErrorMessage = "Full description is Required")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "FullDescription must be between 2 and 500 characters long")]
        public string FullDescription { get; set; }

        [Required(ErrorMessage = "Short desription is Required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "ShortDescription must be between 2 and 100 characters long")]
        public string? ShortDescription { get; set; }

        public decimal? Price { get; set; }

        public PostPriceType? PriceType { get; set; } 

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? MainImageUrl { get; set; }
        public string[]? Tags { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? Country { get; set; }
        public string? Street { get; set; }
        public RentObjectType? RentObjectType { get; set; }
        public int? NumberOfRooms { get; set; }
        public decimal? Area { get; set; }

        public int? Floor { get; set; }
        public WorkloadType? Workload { get; set; } = WorkloadType.Other;
        public WorkLocationType? WorkLocation { get; set; } = WorkLocationType.OnSite;
        public ContractType? Contract { get; set; } = ContractType.CivilContract;

        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? BuildingNumber { get; set; }
        public bool? ExperienceRequired { get; set; } = false;
        public PostSettingsDto? PostSettings { get; set; }
        public SubCategoryDto? SubCategory { get; set; }
        public PromotionDto? Promotion { get; set; }
        public ICollection<PostImageDto>? PostImages { get; set; }
        public ICollection<PostOpinionDto>? PostOpinions { get; set; }
    }
}
