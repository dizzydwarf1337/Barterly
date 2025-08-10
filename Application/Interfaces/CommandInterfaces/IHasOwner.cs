namespace Application.Interfaces.CommandInterfaces;

public interface IHasOwner
{
    Guid OwnerId { get; }
}