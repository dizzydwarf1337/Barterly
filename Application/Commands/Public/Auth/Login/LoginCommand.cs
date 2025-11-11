using Application.Core.MediatR.Requests;

namespace Application.Commands.Public.Auth.Login;

public class LoginCommand : PublicRequest<LoginCommand.Result>
{
    public required string Email { get; set; }
    public required string Password { get; set; }


    public class Result
    {
        public string Token { get; set; }
        public IReadOnlyCollection<string> Roles { get; set; }
    }
    
}