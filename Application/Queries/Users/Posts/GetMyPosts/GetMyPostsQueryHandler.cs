using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetMyPosts;

public class GetMyPostsQueryHandler : IRequestHandler<GetMyPostsQuery, ApiResponse<ICollection<PostPreviewDto>>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IMapper _mapper;

    public GetMyPostsQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }
        
    public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
    {
        return ApiResponse<ICollection<PostPreviewDto>>.Success(
                _mapper.Map<ICollection<PostPreviewDto>>(await _postQueryRepository.GetAllPosts()
                    .Where(x => x.OwnerId == request.AuthorizeData!.UserId && !x.PostSettings.IsDeleted).Include(x=> x.Owner)
                    .ToListAsync(cancellationToken)
                )
            );
    }
}