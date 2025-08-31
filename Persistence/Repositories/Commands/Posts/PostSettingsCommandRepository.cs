using Domain.Enums.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Posts;

public class PostSettingsCommandRepository : BaseCommandRepository<BarterlyDbContext>,
    IPostSettingsCommandRepository
{
    public PostSettingsCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task UpdatePostSettings(Guid postId, CancellationToken token, bool? isHidden, bool? isDeleted,
        PostStatusType? postStatusType, string? RejectionMessage)
    {
        var settings = await _context.PostSettings.FirstOrDefaultAsync(x => x.PostId == postId, token) ??
                       throw new EntityNotFoundException("PostSettings");
        settings.IsHidden = isHidden ?? settings.IsHidden;
        settings.IsDeleted = isDeleted ?? settings.IsDeleted;
        settings.postStatusType = postStatusType ?? settings.postStatusType;
        settings.RejectionMessage = RejectionMessage ?? settings.RejectionMessage;
        _context.PostSettings.Update(settings);
        await _context.SaveChangesAsync(token);
    }
}