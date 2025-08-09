using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetPopularPosts;

public class
    GetPopularPostsQueryHandler : IRequestHandler<GetPopularPostsQuery, ApiResponse<ICollection<PostPreviewDto>>>
{
    private readonly IUserActivityQueryRepository _userActivityQueryRepository;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IMapper _mapper;

    public GetPopularPostsQueryHandler(IUserActivityQueryRepository userActivityQueryRepository, IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _userActivityQueryRepository = userActivityQueryRepository;
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetPopularPostsQuery request,
        CancellationToken cancellationToken)
    {
        var userActivity =
            await _userActivityQueryRepository.GetUserActivityByUserIdAsync(request.AuthorizeData.UserId,
                cancellationToken);
        var city = userActivity.MostViewedCities.Count() > 0
            ? userActivity.MostViewedCities[0].ToString()
            : null;
        var posts = _postQueryRepository.GetAllPosts();

        posts = posts.OrderBy(x => x.ViewsCount);
        
        if(city is not null)
        {
            posts = posts.Where(x=>x.City.ToLower() == city.ToLower());
        }

        return ApiResponse<ICollection<PostPreviewDto>>.Success(_mapper.Map<List<PostPreviewDto>>(await posts.ToListAsync(cancellationToken)));



    }
}