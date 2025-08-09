using Application.Core.MediatR.Requests;
using Application.Core.Validators.Interfaces;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : AuthorizedRequest<TResponse>

{
    private readonly IAccountOwnershipValidator _accountValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IOpinionOwnershipValidator _opinionOwnershipValidator;
    private readonly IPostOwnershipValidator _postOwnershipValidator;

    public ValidationBehavior(
        IAccountOwnershipValidator accountValidator,
        IOpinionOwnershipValidator opinionOwnershipValidator,
        IPostOwnershipValidator postOwnershipValidator,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _accountValidator = accountValidator;
        _opinionOwnershipValidator = opinionOwnershipValidator;
        _postOwnershipValidator = postOwnershipValidator;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var userId = request.AuthorizeData.UserId;
        var hasAuthorize = request.AuthorizeData.Endpoint.Metadata?.GetMetadata<AuthorizeAttribute>() != null;
        if (!hasAuthorize) return await next(cancellationToken);

        _logger.LogDebug("Entering vlidation behavior for request type {0} and user {1}", typeof(TRequest).Name,
            userId);

        var requestType = request.GetType();

        if (request is IHasOwner hasOwner) await _accountValidator.ValidateAccountOwnership(userId, hasOwner.OwnerId);

        if (request is IPostOwner postOwner)
            await _postOwnershipValidator.ValidatePostOwnership(userId, postOwner.PostId);

        if (request is IOpinionOwner opinionOwner)
            await _opinionOwnershipValidator.ValidateOpinionOwnership(userId, opinionOwner.OpinionId);

        return await next(cancellationToken);
    }
}