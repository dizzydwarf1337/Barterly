using Application.DTOs.Posts;

namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> CreatePost(CreatePostDto post);
        Task<PostDto> UpdatePost(EditPostDto post);
        Task<PostDto> UploadImages(ImagesDto imagesDto);
        Task<PostDto> GetPostById(Guid postId, Guid? userId = null);
        Task<PostDto> GetPostByIdAdmin(Guid postId);
        Task<ICollection<PostPreviewDto>> GetUserFavouritePostsPaginated(Guid categoryId, int PageSize, int PageNumber);
        Task<ICollection<PostPreviewDto>> GetPostsByUserIdPaginated(Guid userId, int PageSize, int PageNumber, Guid? currentUserId = null); // user's created posts

    }
}
