namespace Domain.Interfaces.Queries.User;

public interface IUserQueryRepository
{
    Task<Entities.Users.User> GetUserAsync(Guid id, CancellationToken token);
    Task<Entities.Users.User> GetUserByEmail(string email, CancellationToken token);
    IQueryable<Entities.Users.User> GetUsers();
}