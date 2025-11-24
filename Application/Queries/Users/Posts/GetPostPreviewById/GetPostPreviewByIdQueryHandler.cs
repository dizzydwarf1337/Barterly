using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetPostPreviewById;

public class GetPostPreviewByIdQueryHandler : IRequestHandler<GetPostPreviewByIdQuery, ApiResponse<PostPreviewDto>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IMapper _mapper;

    public GetPostPreviewByIdQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }
    public async Task<ApiResponse<PostPreviewDto>> Handle(GetPostPreviewByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetAllPosts()
            .Where(post =>
                post.Id == request.PostId &&
                (
                    (post.OwnerId == request.AuthorizeData!.UserId && !post.PostSettings.IsDeleted)
                    ||
                    (!post.PostSettings.IsHidden && !post.PostSettings.IsDeleted)
                )
            ).FirstOrDefaultAsync(cancellationToken);
        if(post == null) return ApiResponse<PostPreviewDto>.Failure("Post not found", 404);
        
        return ApiResponse<PostPreviewDto>.Success(_mapper.Map<PostPreviewDto>(post));
    }
}