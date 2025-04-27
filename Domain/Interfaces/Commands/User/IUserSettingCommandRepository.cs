using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserSettingCommandRepository
    {
        Task CreateUserSettings(UserSettings settings);
        Task DeleteUserSettings(Guid settingsId);
        Task UpdateUserSettings(UserSettings settings);
    }
}
