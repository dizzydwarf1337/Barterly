using Application.DTOs.Posts;

namespace Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<ICollection<PostPreviewDto>> GetFeed(int page, Guid? userId = null);
        Task<ICollection<PostPreviewDto>> GetPopularPosts(int count, string? city);
        Task<ICollection<PostPreviewDto>> GetFiltredPosts(int? pageCount, int? page, Guid? subCategoryId = null, string? city = null, string? region = null);
        Task<ICollection<PostPreviewDto>> GetPromotedPosts(int count, List<string>? categories = null, List<string>? cities = null);
    }
}
