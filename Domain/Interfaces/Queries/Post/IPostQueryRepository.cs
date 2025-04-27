namespace Domain.Interfaces.Queries.Post
{
    public interface IPostQueryRepository
    {
        Task<Entities.Posts.Post> GetPostById(Guid postId);
        Task<Entities.Posts.Post> GetPostByIdAdmin(Guid postId);
        Task<ICollection<Entities.Posts.Post>> GetPostsByOwnerIdPaginated(Guid ownerId, int pageCount, int page, Guid? currentUserId = null);
        Task<ICollection<Entities.Posts.Post>> GetFiltredPostsAsync(int? pageCount, int? page, Guid? subCategoryId = null, string? city = null, string? region = null);
        Task<ICollection<Entities.Posts.Post>> GetUserFavouritePostsPaginated(Guid userId, int PageSize, int PageNumber);
        Task<ICollection<Entities.Posts.Post>> GetFeed(List<string> categories, List<string> cities, int pageCount, int Page);
        Task<ICollection<Entities.Posts.Post>> GetPromotedPosts(int count, List<string>? categories, List<string>? cities);
        Task<ICollection<Entities.Posts.Post>> GetPopularPosts(int count, string? city);
    }
}
