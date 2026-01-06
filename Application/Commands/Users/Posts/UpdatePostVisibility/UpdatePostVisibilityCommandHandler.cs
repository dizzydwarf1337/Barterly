using Application.Core.ApiResponse;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.Posts.UpdatePostVisibility;

public class UpdatePostVisibilityCommandHandler : IRequestHandler<UpdatePostVisibilityCommand, ApiResponse<Unit>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

    public UpdatePostVisibilityCommandHandler(IPostQueryRepository postQueryRepository,
        IPostSettingsCommandRepository postSettingsCommandRepository)
    {
        _postQueryRepository = postQueryRepository;
        _postSettingsCommandRepository = postSettingsCommandRepository;
    }
    
    public async Task<ApiResponse<Unit>> Handle(UpdatePostVisibilityCommand request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetAllPosts()
            .Include(x => x.PostSettings)
            .FirstOrDefaultAsync(x => x.Id == request.PostId && x.OwnerId == request.AuthorizeData!.UserId, cancellationToken);
        if(post == null || post.PostSettings.postStatusType == PostStatusType.UnderReview)
            return ApiResponse<Unit>.Failure("Post not found or under review", 404);

        await _postSettingsCommandRepository.UpdatePostSettings(post.PostSettingsId, cancellationToken,
            !post.PostSettings.IsHidden);
        
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}