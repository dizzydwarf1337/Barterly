using Application.Core.MediatR.Requests;

namespace Application.Queries.Public.Users.GetUserById;

public class GetUserByIdQuery : PublicRequest<GetUserByIdQuery.Result>
{
    
    public Guid Id { get; set; }

    public record Result(Guid Id, string FirstName, string LastName, string ProfilePicturePath);
}