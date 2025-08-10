using Domain.Entities.Posts;

namespace Domain.Interfaces.Queries.Post;

public interface IPostSettingsQueryRepository
{
    public Task<PostSettings> GetPostSettingsById(Guid settingsId, CancellationToken token);
    public Task<PostSettings> GetPostSettingsByPostId(Guid postId, CancellationToken token);
}