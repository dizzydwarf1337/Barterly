using Application.Core.ApiResponse;
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
            .Where(x => x.Id == request.PostId && x.OwnerId == request.AuthorizeData!.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        if(post == null)
            return ApiResponse<Unit>.Failure("Post not found", 404);

        await _postSettingsCommandRepository.UpdatePostSettings(post.PostSettingsId, cancellationToken,
            !post.PostSettings.IsHidden);
        
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}