using Application.Core.ApiResponse;
using Application.Core.MediatR.Requests;

namespace Application.Queries.Public.Users.GetMe;

public class GetMeQuery : AuthorizedRequest<ApiResponse<GetMeQuery.Result>>
{
    public class Result
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? Role { get; set; }
        public int NotificationCount { get; set; } = 0;
        public IReadOnlyCollection<Guid> FavPostIds { get; set; } 
    }
}