using Application.Core.Factories.Interfaces;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Enums.Posts;
using Domain.Exceptions.DataExceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Core.Factories.PostFactory;

public class PostFactory : IPostFactory
{
    public Post CreatePost(Guid subCategoryId,
        Guid ownerId,
        string postType,
        string title,
        string fullDescription,
        string shortDescription,
        CancellationToken token,
        PostCurrency currency,
        string? city = null,
        string? region = null,
        string? country = null,
        string? street = null,
        decimal? price = null,
        PostPriceType? postPriceType = null,
        string[]? tags = null,
        IFormFile? mainImage = null,
        IFormFile[]? secondaryImages = null,
        RentObjectType? rentObjectType = null,
        int? numberOfRooms = null,
        decimal? area = null,
        int? floor = null,
        WorkloadType? workload = null,
        WorkLocationType? workLocation = null,
        decimal? minSalary = null,
        decimal? maxSalary = null,
        string? buildingNumber = null,
        bool? experienceRequired = null
)
    {
        Post post;

        switch (postType)
        {
            case "Rent":
            {
                post = new RentPost
                {
                    OwnerId = ownerId,
                    Title = title,
                    FullDescription = fullDescription,
                    ShortDescription = shortDescription,
                    City = city,
                    Region = region,
                    Country = country,
                    Street = street,
                    Price = price,
                    Tags = tags ?? [],
                    RentObjectType = rentObjectType ?? RentObjectType.House,
                    NumberOfRooms = numberOfRooms,
                    Area = area,
                    Floor = floor,
                    PriceType = postPriceType ?? PostPriceType.PerMonth,
                    HouseNumber = buildingNumber,
                    Currency = currency,
                };
                break;
            }
            case "Work":
            {
                post = new WorkPost
                {
                    OwnerId = ownerId,
                    Title = title,
                    FullDescription = fullDescription,
                    ShortDescription = shortDescription,
                    City = city,
                    Region = region,
                    Country = country,
                    Street = street,
                    Price = price,
                    Tags = tags ?? [],
                    Workload = workload ?? WorkloadType.FullTime,
                    WorkLocation = workLocation ?? WorkLocationType.OnSite,
                    MinSalary = minSalary,
                    MaxSalary = maxSalary,
                    ExperienceRequired = experienceRequired ?? false
                };
                break;
            }
            case "Common":
            {
                post = new CommonPost
                {
                    OwnerId = ownerId,
                    Title = title,
                    FullDescription = fullDescription,
                    ShortDescription = shortDescription,
                    City = city,
                    Region = region,
                    Country = country,
                    Street = street,
                    Price = price,
                    Tags = tags ?? [],
                    PriceType = postPriceType ?? PostPriceType.PerItem
                };
                break;
            }
            default: throw new InvalidDataProvidedException("PostType", "PostDto", "PostFactory.CreatePost");
        }

        post.SubCategoryId = subCategoryId;
        //Promotion 
        var postPromotion = new Promotion
        {
            PostId = post.Id,
            Post = post
        };


        //Settings
        var postSettings = new PostSettings
        {
            PostId = post.Id,
            Post = post
        };
        post.PostSettingsId = postSettings.PostId;
        post.PostSettings = postSettings;
        post.PromotionId = postPromotion.Id;
        post.Promotion = postPromotion;
        return post;
    }
    
}