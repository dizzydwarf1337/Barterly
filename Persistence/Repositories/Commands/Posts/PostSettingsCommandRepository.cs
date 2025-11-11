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

    public async Task UpdatePostSettings(Guid settingsId, CancellationToken token, bool? isHidden, bool? isDeleted,
        PostStatusType? postStatusType, string? rejectionMessage)
    {
        var settings = await _context.PostSettings.FirstOrDefaultAsync(x => x.Id == settingsId, token) ??
                       throw new EntityNotFoundException("PostSettings");
        settings.IsHidden = isHidden ?? settings.IsHidden;
        settings.IsDeleted = isDeleted ?? settings.IsDeleted;
        settings.postStatusType = postStatusType ?? settings.postStatusType;
        settings.RejectionMessage = rejectionMessage ?? settings.RejectionMessage;
        _context.PostSettings.Update(settings);
        await _context.SaveChangesAsync(token);
    }
}