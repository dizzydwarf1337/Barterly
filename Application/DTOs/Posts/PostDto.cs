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
        public string Id { get; set; } 

        public string OwnerId { get; set; }

        public string Title { get; set; }

        public string FullDescription { get; set; }

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
        public PostSettingsDto PostSettings { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public PromotionDto Promotion { get; set; }
        public ICollection<PostImageDto>? PostImages { get; set; }
        public ICollection<PostOpinionDto>? PostOpinions { get; set; }
    }
}
