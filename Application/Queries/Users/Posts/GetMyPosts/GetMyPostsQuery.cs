using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetMyPosts;

public class GetMyPostsQuery : UserRequest<ICollection<PostPreviewDto>> { }