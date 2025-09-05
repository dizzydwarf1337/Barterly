using Application.Core.MediatR.Requests;

namespace Application.Queries.Admins.Users.GetUserById;

public class GetUserByIdQuery : AdminRequest<GetUserByIdQuery.Result>
{
    
    public Guid Id { get; set; }
    
    public record Result(UserData UserData, UserSettings UserSettings);

    public record UserData
    (
        Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Bio,
    string? Country,
    string? City,
    string? Street, 
    string? HouseNumber, 
    string? PostalCode,
    string? ProfilePicturePath, 
    DateTime CreatedAt, 
    DateTime LastSeen    
    );

    public record UserSettings
    (
     Guid Id, 
     bool IsHidden, 
     bool IsDeleted,
     bool IsBanned,
     bool IsPostRestricted,
     bool IsOpinionRestricted,
     bool IsChatRestricted
    );
    
}