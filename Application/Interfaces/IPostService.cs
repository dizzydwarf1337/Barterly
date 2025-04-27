using Application.DTOs.Posts;

namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> CreatePost(CreatePostDto post);
        Task UpdatePost(PostDto post);
        Task<PostDto> GetPostById(Guid postId);
        Task<ICollection<PostPreviewDto>> GetUserFavouritePostsPaginated(Guid categoryId, int PageSize, int PageNumber);
        Task<ICollection<PostPreviewDto>> GetPostsByUserIdPaginated(Guid userId, int PageSize, int PageNumber, Guid? currentUserId = null); // user's created posts

    }
}
