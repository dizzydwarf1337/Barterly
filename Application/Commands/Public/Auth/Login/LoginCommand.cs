using Application.Core.MediatR.Requests;

namespace Application.Commands.Public.Auth.Login;

public class LoginCommand : PublicRequest<LoginCommand.Result>
{
    public required string Email { get; set; }
    public required string Password { get; set; }

    public class Result
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? token { get; set; }
        public string? Role { get; set; }
    }
}