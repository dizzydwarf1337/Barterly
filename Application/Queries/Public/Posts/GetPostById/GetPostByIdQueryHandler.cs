using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Queries.Public.Posts.GetPostById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ApiResponse<PostDto>>
{
    private readonly IMapper _mapper;
    private readonly IPostQueryRepository _postQueryRepository;

    public GetPostByIdQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetPostById(request.PostId, cancellationToken);
        if (post.PostSettings.IsDeleted || post.PostSettings.IsHidden)
            return ApiResponse<PostDto>.Failure("Post not found or is hidden/deleted.", 404);
        return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post));
    }
}