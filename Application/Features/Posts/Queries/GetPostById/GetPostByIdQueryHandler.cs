using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Features.Posts.Events.PostVisitedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Features.Posts.Queries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ApiResponse<PostDto>>
    {
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetPostByIdQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper,IMediator mediator)
        {
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ApiResponse<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var postId = Guid.Parse(request.postId);

            Guid? userGuid = null;
            if (!string.IsNullOrEmpty(request.userId))
                userGuid = Guid.Parse(request.userId);

            var post = await _postQueryRepository.GetPostById(postId, userGuid);

            if (userGuid.HasValue)
                await _mediator.Publish(new PostVisitedEvent { PostId = post.Id, UserId = request.userId });

            return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post));
        }
    }
}
