using Application.Core.MediatR.Requests;
using Application.Core.Validators.Interfaces;
using Application.Interfaces.CommandInterfaces;
using MediatR;

namespace Application.Core.Behaviors;

public class UserPermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : UserRequest<TResponse>
{
    private readonly IAuthChecker _authChecker;

    public UserPermissionBehavior(IAuthChecker authChecker)
    {
        _authChecker = authChecker;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IPostOwner postOwner)
            await _authChecker.CheckPostPermission(request.AuthorizeData.UserId);
        else if (request is IOpinionOwner opinionOwner)
            await _authChecker.CheckCommentPermission(request.AuthorizeData.UserId);

        return await next(cancellationToken);
    }
}