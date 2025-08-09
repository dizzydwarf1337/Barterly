namespace Application.Core.Validators.Interfaces;

public interface IAccountOwnershipValidator
{
    Task ValidateAccountOwnership(Guid userId, Guid accountId);
}