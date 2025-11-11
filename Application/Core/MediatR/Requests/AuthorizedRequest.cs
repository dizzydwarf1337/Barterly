using System.Text.Json.Serialization;
using Domain.Enums.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Core.MediatR.Requests;

public class AuthorizedRequest<T> : IRequest<T>
{
    [JsonIgnore]
    public AuthorizeData? AuthorizeData { get; set; } = default!;
}

public class AuthorizeData
{
    public AuthorizeData(Guid userId, ICollection<UserRoles> role, string token, Endpoint endpoint)
    {
        UserId = userId;
        Role = role;
        Token = token;
        Endpoint = endpoint;
    }

    public Guid UserId { get; set; }
    public ICollection<UserRoles> Role { get; set; }
    public string Token { get; set; }

    public Endpoint Endpoint { get; set; }
}