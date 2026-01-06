using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetUsersPosts;

public class GetUsersPostsQueryHandler : IRequestHandler<GetUsersPostsQuery, ApiResponse<ICollection<PostPreviewDto>>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IMapper  _mapper;

    public GetUsersPostsQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetUsersPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postQueryRepository.GetAllPosts()
            .Where(x => x.OwnerId == request.UserId && !(x.PostSettings.IsDeleted || x.PostSettings.IsHidden))
            .Include(x => x.Owner)
            .ToListAsync(cancellationToken);
        return ApiResponse<ICollection<PostPreviewDto>>.Success(_mapper.Map<ICollection<PostPreviewDto>>(posts));
    }
}