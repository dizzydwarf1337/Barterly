using Domain.Enums.Posts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Posts
{
    public class CreatePostDto
    {
        [Required]
        public required string SubCategoryId { get; set; }
        [Required]
        public required string OwnerId { get; set; }
        [Required]
        public required string PostType { get; set; }
        [Required(ErrorMessage ="Title is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 50 characters long")]
        public required string Title { get; set; }

        [Required(ErrorMessage ="City is Required")]
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? Street { get; set; }
        
        [Required(ErrorMessage ="Full description is Required")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "FullDescription must be between 2 and 500 characters long")]
        public required string FullDescription { get; set; }

        [Required(ErrorMessage ="Short desription is Required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "ShortDescription must be between 2 and 100 characters long")]
        public required string ShortDescription { get; set; }
        public decimal? Price { get; set; }
        public PostPriceType? PostPriceType { get; set; } = null;
        public string[]? Tags { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile[]? SecondaryImages { get; set; }
        public RentObjectType? RentObjectType { get; set; }
        public int? NumberOfRooms { get; set; }
        public decimal? Area { get; set; }
        public int? Floor { get; set; }
        public WorkloadType? Workload { get; set; }
        public WorkLocationType? WorkLocation { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? BuildingNumber {  get; set; }
        public bool? ExperienceRequired { get; set; }
    }
}
