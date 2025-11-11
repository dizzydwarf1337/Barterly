using Domain.Entities.Posts;
using Domain.Enums.Posts;
using Microsoft.AspNetCore.Http;

namespace Application.Core.Factories.Interfaces;

public interface IPostFactory
{
    public Post CreatePost(
        Guid subCategoryId,
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
    );
}