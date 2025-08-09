namespace Domain.Interfaces.Commands.User;

public interface IUserCommandRepository
{
    Task AddUserAsync(Entities.Users.User user, CancellationToken token);
    Task UpdateUserAsync(Entities.Users.User user, CancellationToken token);
    Task DeleteUser(Guid userId, CancellationToken token);
    Task UploadPicture(Guid id, string PicPath, CancellationToken token);
}