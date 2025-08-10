using Application.Core.ApiResponse;
using Application.Events.Posts.PostDeletedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Users.Posts.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

    public DeletePostCommandHandler(IPostSettingsCommandRepository postSettingsCommandRepository, IMapper mapper,
        IMediator mediator, ILogService logService)
    {
        _postSettingsCommandRepository = postSettingsCommandRepository;
        _mapper = mapper;
        _mediator = mediator;
        _logService = logService;
    }

    public async Task<ApiResponse<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        await _postSettingsCommandRepository.UpdatePostSettings(request.PostId, true, true,
            PostStatusType.Deleted, null, cancellationToken);
        await _mediator.Publish(new PostDeletedEvent { postId = request.PostId });
        await _logService.CreateLogAsync($"Post deleted id: {request.PostId}", cancellationToken,
            LogType.Information, postId: request.PostId);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}