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
        var favIds = (await _favPostQueryRepository
                .GetUserFavPostsByUserIdAsync(request.AuthorizeData.UserId, cancellationToken))
            .Select(x => x.PostId);

        var query = _postQueryRepository.GetAllPosts()
            .Include(x => x.PostSettings)
            .Where(x => favIds.Contains(x.Id) && !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden);

        var totalCount = await query.CountAsync(cancellationToken);

        var posts = await query
            .Skip((request.FilterBy.PageNumber - 1) * request.FilterBy.PageSize)
            .Take(request.FilterBy.PageSize)
            .ToListAsync(cancellationToken);

        return ApiResponse<GetFavPostsQuery.Result>.Success(new GetFavPostsQuery.Result
        {
            Items = _mapper.Map<List<PostPreviewDto>>(posts),
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.FilterBy.PageSize)
        });
    }
}