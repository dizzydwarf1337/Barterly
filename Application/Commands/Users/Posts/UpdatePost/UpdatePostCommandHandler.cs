using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Events.Posts.PostUpdatedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Users.Posts.UpdatePost;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ApiResponse<PostDto>>
{
    private readonly ILogService _logService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostCommandRepository _postCommandRepository;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

    public UpdatePostCommandHandler(IPostCommandRepository postCommandRepository,
        IPostSettingsCommandRepository postSettingsCommandRepository, IMapper mapper, IMediator mediator,
        ILogService logService)
    {
        _postCommandRepository =
            postCommandRepository ?? throw new ArgumentNullException(nameof(postCommandRepository));
        _postSettingsCommandRepository = postSettingsCommandRepository ??
                                         throw new ArgumentNullException(nameof(postSettingsCommandRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    public async Task<ApiResponse<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = _mapper.Map<Post>(request.Post);
        await _postCommandRepository.UpdatePostAsync(post, cancellationToken);
        await _postSettingsCommandRepository.UpdatePostSettings(post.Id, true, false, PostStatusType.ReSubmitted,
            "", cancellationToken);
        await _mediator.Publish(new PostUpdatedEvent { PostId = post.Id, UserId = post.OwnerId });
        await _logService.CreateLogAsync($"Post updated title: {post.Title}", cancellationToken,
            LogType.Information, postId: post.Id, userId: post.OwnerId);
        return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post));
    }
}