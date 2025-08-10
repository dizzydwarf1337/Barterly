namespace Domain.Exceptions.BusinessExceptions;

public class AccessForbiddenException : Exception
{
    public AccessForbiddenException(string? place, string? userId, string? reason)
        : base($"Access forbidden for {userId} in {place}")
    {
        UserId = userId;
        Reason = reason;
    }

    public string? UserId { get; }
    public string? Reason { get; }
}