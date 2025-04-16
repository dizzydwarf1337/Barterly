namespace Domain.Interfaces.Commands.User
{
    public interface IUserCommandRepository
    {
        Task AddUserAsync(Entities.Users.User user);
        Task UpdateUserAsync(Entities.Users.User user);
        Task DeleteUser(Guid userId);
        Task UploadPicture(Guid id, string PicPath);
    }
}
