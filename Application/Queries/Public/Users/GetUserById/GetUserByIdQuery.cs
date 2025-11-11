using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Users.GetUserById;

public class GetUserByIdQuery : PublicRequest<GetUserByIdQuery.Result>
{
    
    public Guid Id { get; set; }

    public record Result(Guid Id, string FirstName, DateTime? CreatedAt, string LastName, string ProfilePicturePath, ICollection<PostPreviewDto> Posts);
}