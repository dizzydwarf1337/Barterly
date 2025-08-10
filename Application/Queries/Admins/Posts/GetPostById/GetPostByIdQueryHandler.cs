using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Queries.Admins.Posts.GetPostById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ApiResponse<PostDto>>
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

    public async Task<ApiResponse<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetPostById(request.PostId, cancellationToken);

        return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post));
    }
}