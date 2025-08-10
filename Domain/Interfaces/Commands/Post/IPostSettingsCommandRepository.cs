using Domain.Enums.Posts;

namespace Domain.Interfaces.Commands.Post;

public interface IPostSettingsCommandRepository
{
    public Task UpdatePostSettings(Guid postId, bool? isHidden, bool? isDeleted, PostStatusType? postStatusType,
        string? rejectionMessage, CancellationToken token);
}