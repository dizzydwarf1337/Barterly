using Application.Core.Validators.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;

namespace Application.Core.Validators.Implementation;

public class AuthChecker : IAuthChecker
{
    private readonly IUserSettingQueryRepository _userSettingsQueryRepository;

    public AuthChecker(IUserSettingQueryRepository userSettingsQueryRepository)
    {
        _userSettingsQueryRepository = userSettingsQueryRepository;
    }

    public async Task CheckPostPermission(Guid ownerId)
    {
        var settings = await _userSettingsQueryRepository.GetUserSettingByUserIdAsync(ownerId, CancellationToken.None);
        if (settings.IsPostRestricted)
            throw new AccessForbiddenException("AuthChecker.CheckPostPermission", ownerId.ToString(),
                "User is restricted from creating posts");
    }

    public async Task CheckCommentPermission(Guid ownerId)
    {
        var settings = await _userSettingsQueryRepository.GetUserSettingByUserIdAsync(ownerId, CancellationToken.None);
        if (settings.IsOpinionRestricted)
            throw new AccessForbiddenException("AuthChecker.CheckCommentPermission",
                ownerId.ToString(), "User is restricted from creating comments");
    }
}