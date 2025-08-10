namespace Domain.Exceptions.DataExceptions;

public class InvalidDataProvidedException : Exception
{
    public InvalidDataProvidedException(string? message) : base(message)
    {
    }

    public InvalidDataProvidedException(string? field, string? entity, string? place) : base(
        $"Invalid {field} provided for {entity} in {place}")
    {
    }
}