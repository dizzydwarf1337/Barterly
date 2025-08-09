namespace Application.Core.Validators.Interfaces;

public interface IPostOwnershipValidator
{
    Task ValidatePostOwnership(Guid userId, Guid postId);
}