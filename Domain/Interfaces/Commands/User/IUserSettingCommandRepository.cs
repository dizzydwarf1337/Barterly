using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User;

public interface IUserSettingCommandRepository
{
    Task CreateUserSettings(UserSettings settings, CancellationToken token);
    Task DeleteUserSettings(Guid settingsId, CancellationToken token);
    Task UpdateUserSettings(UserSettings settings, CancellationToken token);
    Task UpdateUserSettingsByUserId(Guid userId, bool isHidden, bool isDeleted, bool isBanned, bool isChatRestricted, bool isOpinionRestricted, bool isPostRestricted, CancellationToken token );
}