namespace Domain.Interfaces.Queries.User
{
    public interface IUserQueryRepository
    {
        Task<Entities.Users.User> GetUserAsync(Guid id);
        Task<Entities.Users.User> GetUserByEmail(string email);
        Task<ICollection<Entities.Users.User>> GetUsersAsync();
        Task<ICollection<Entities.Users.User>> GetUsersByCityAsync(string city);
        Task<ICollection<Entities.Users.User>> GetUsersByCountryAsync(string country);
        Task<ICollection<Entities.Users.User>> GetNewUsersAsync(DateTime date);
    }
}
