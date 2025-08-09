using Domain.Entities.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Posts;

public class PostSettingsQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostSettingsQueryRepository
{
    public PostSettingsQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<PostSettings> GetPostSettingsById(Guid settingsId, CancellationToken token)
    {
        var settings = await _context.PostSettings.FindAsync(settingsId, token) ??
                       throw new EntityNotFoundException("Post settings");
        return settings;
    }

    public async Task<PostSettings> GetPostSettingsByPostId(Guid postId, CancellationToken token)
    {
        var settings = await _context.PostSettings.FirstOrDefaultAsync(x => x.PostId == postId, token) ??
                       throw new EntityNotFoundException("Post settings");
        return settings;
    }
}