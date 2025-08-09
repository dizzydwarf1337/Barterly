using Application.Core.Validators.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;

namespace Application.Core.Validators.Implementation;

public class PostOwnershipValidator : IPostOwnershipValidator
{
    private readonly IPostQueryRepository _postQueryRepository;

    public PostOwnershipValidator(IPostQueryRepository postQueryRepository)
    {
        _postQueryRepository = postQueryRepository;
    }

    public async Task ValidatePostOwnership(Guid userId, Guid postId)
    {
        var post = await _postQueryRepository.GetPostById(postId, CancellationToken.None) ?? throw new AccessForbiddenException(
            "ValidatePostOwnership", userId.ToString(),
            "User haven't an access to this post, or post doesn't exists");
        if (post.OwnerId != userId)
            throw new AccessForbiddenException("ValidatePostOwnership", userId.ToString(),
                "OwnerId and userId mismatch");
    }
}