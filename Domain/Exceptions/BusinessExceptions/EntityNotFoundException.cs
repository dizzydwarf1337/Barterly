namespace Domain.Exceptions.BusinessExceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entity) : base($"{entity} not found")
    {
    }
}