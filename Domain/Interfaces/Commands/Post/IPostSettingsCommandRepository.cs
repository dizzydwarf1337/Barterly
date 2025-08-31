using Domain.Enums.Posts;

namespace Domain.Interfaces.Commands.Post;

public interface IPostSettingsCommandRepository
{
    public Task UpdatePostSettings(Guid postId, CancellationToken token, bool? isHidden = false, bool? isDeleted = false, PostStatusType? postStatusType = PostStatusType.Published,
        string? rejectionMessage = "");
}