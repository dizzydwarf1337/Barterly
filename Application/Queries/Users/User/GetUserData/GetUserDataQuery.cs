using Application.Core.MediatR.Requests;

namespace Application.Queries.Public.Users.GetUserData;

public class GetUserDataQuery : UserRequest<GetUserDataQuery.Result>
{
    public Guid UserId { get; set; }
    public record Result(
        Guid Id,
        string FirstName,
        string LastName,
        string? Bio,
        string? Country,
        string? City,
        string? Street,
        string? HouseNumber,
        string? PostalCode,
        string? ProfilePicturePath);
}