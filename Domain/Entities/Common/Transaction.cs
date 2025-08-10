namespace Domain.Entities.Common;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
}