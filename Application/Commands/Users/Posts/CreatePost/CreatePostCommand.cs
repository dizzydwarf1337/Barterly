using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Interfaces.CommandInterfaces;
using Domain.Enums.Posts;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Users.Posts.CreatePost;

public class CreatePostCommand : UserRequest<PostDto>, IHasOwner
{
    public Guid SubCategoryId { get; set; }
    public string PostType { get; set; }
    public string Title { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? Street { get; set; }
    public string FullDescription { get; set; }
    public string ShortDescription { get; set; }
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
    public string? BuildingNumber { get; set; }
    public bool? ExperienceRequired { get; set; }
    public Guid OwnerId { get; set; }
}