using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetFeed;

public class GetFeedQueryHanlder : IRequestHandler<GetFeedQuery, ApiResponse<ICollection<PostPreviewDto>>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IMapper _mapper;
    public GetFeedQueryHanlder(IPostQueryRepository postQueryRepository, IMapper mapper) { 
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetFeedQuery request,
        CancellationToken cancellationToken)
    {
        var posts = _postQueryRepository.GetAllPosts();
        posts = posts.OrderByDescending(x => x.VisitedPosts.Count).OrderByDescending(x => x.ViewsCount);
        return ApiResponse<ICollection<PostPreviewDto>>.Success(_mapper.Map<List<PostPreviewDto>>(await posts.ToListAsync(cancellationToken)));
    }
}