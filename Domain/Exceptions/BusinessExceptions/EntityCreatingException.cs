namespace Domain.Exceptions.BusinessExceptions;

public class EntityCreatingException : Exception
{
    public EntityCreatingException(string? entity, string? place) : base($"Error creating new {entity} in {place}")
    {
    }
}