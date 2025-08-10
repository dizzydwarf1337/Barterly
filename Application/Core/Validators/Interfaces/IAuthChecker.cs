namespace Application.Core.Validators.Interfaces;

public interface IAuthChecker
{
    Task CheckPostPermission(Guid ownerId);
    Task CheckCommentPermission(Guid ownerId);
}