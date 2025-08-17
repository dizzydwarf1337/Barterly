using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetFavPosts;

public class GetFavPostsQueryHandler : IRequestHandler<GetFavPostsQuery, ApiResponse<GetFavPostsQuery.Result>>
{
    private readonly IMapper _mapper;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IUserFavPostQueryRepository _favPostQueryRepository;

    public GetFavPostsQueryHandler(IMapper mapper, IPostQueryRepository postQueryRepository,
        IUserFavPostQueryRepository favPostQueryRepository)
    {
        _mapper = mapper;
        _postQueryRepository = postQueryRepository;
        _favPostQueryRepository = favPostQueryRepository;
    }
    
    public async Task<ApiResponse<GetFavPostsQuery.Result>> Handle(GetFavPostsQuery request, CancellationToken cancellationToken)
    {
        var favIds = (await _favPostQueryRepository.GetUserFavPostsByUserIdAsync(request.AuthorizeData.UserId, cancellationToken)).Select(x=>x.PostId);
        var posts = await _postQueryRepository.GetAllPosts().Include(x => x.PostSettings)
            .Where(x => favIds.Contains(x.Id) && !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden)
            .Skip((request.FilterBy.PageNumber - 1) * request.FilterBy.PageSize)
            .Take(request.FilterBy.PageSize)
            .ToListAsync(cancellationToken);
        
        return ApiResponse<GetFavPostsQuery.Result>.Success(new GetFavPostsQuery.Result()
        {
            Posts = _mapper.Map<List<PostPreviewDto>>(posts),
            TotalCount = posts.Count,
            TotalPages = posts.Count / request.FilterBy.PageSize,
        });

    }
}