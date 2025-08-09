namespace Domain.Exceptions.BusinessExceptions;

public class ConfigException : Exception
{
    public ConfigException(string? message) : base(message)
    {
    }
}