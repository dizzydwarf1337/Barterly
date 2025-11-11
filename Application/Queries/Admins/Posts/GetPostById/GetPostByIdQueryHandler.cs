using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Admins.Posts.GetPostById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ApiResponse<GetPostByIdQuery.Result>>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostQueryRepository _postQueryRepository;

    public GetPostByIdQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper, IMediator mediator)
    {
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<ApiResponse<GetPostByIdQuery.Result>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetAllPosts()
            .Include(x=>x.Owner)
            .Include(x=>x.PostImages)
            .FirstOrDefaultAsync(x=> x.Id == request.PostId, cancellationToken);
        
        if (post == null)
            return ApiResponse<GetPostByIdQuery.Result>.Failure("Post not found");
        
        return ApiResponse<GetPostByIdQuery.Result>.Success(
            new GetPostByIdQuery.Result(
                    _mapper.Map<PostDto>(post),
                    _mapper.Map<PostSettingsDto>(post.PostSettings),
                    _mapper.Map<PostOpinionDto[]>(post.PostOpinions),
                    new GetPostByIdQuery.OwnerData(post.OwnerId, post.Owner.FirstName, post.Owner.LastName)
                )
            );
    }
}