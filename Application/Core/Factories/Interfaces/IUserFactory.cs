using Domain.Entities.Users;
using Domain.Enums.Users;
using Google.Apis.Auth;

namespace Application.Core.Factories.Interfaces;

public interface IUserFactory
{
    public Task<User> CreateUser(string FirstName, string LastName, string Email, string Password, UserRoles Role);
    public Task<User> CreateUser(GoogleJsonWebSignature.Payload payload);
}