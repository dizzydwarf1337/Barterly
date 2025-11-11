using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Application.Core.Hub;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var id = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine($"[UserIdProvider] Extracted id: {id ?? "null"}");
        return id;
    }
}