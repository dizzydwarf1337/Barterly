namespace Application.Core.Validators.Interfaces;

public interface IOpinionOwnershipValidator
{
    public Task ValidateOpinionOwnership(Guid userId, Guid opinionId);
}