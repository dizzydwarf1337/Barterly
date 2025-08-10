namespace Domain.Exceptions.BusinessExceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string entity) : base($"{entity} already exists")
    {
    }
}