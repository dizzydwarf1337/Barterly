using System.Security.Claims;
using Application.Core.ApiResponse;
using Application.Core.MediatR.Requests;
using Domain.Enums.Users;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Core.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : AuthorizedRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;

    public AuthorizationBehavior(ILogger<AuthorizationBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning("Entering authorization behavior for request type {0}", typeof(TRequest).Name);
        var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        throw new AccessForbiddenException("Authorization behavior", null, "User id is null");
        var userRoles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
                            .Select(x =>
                                x.Value == "User" ? UserRoles.User :
                                x.Value == "Moderator" ? UserRoles.Moderator :
                                x.Value == "Admin" ? UserRoles.Admin :
                                throw new AccessForbiddenException("Authorization behavior", null,
                                    "role not found")).ToList() ??
                        throw new AccessForbiddenException("Authorization behavior", userIdStr,
                            "User has no roles");
        var endpoint = _httpContextAccessor.HttpContext?.GetEndpoint() ??
                       throw new AccessForbiddenException("Authorization behavior", userIdStr,
                           "Endpoint not found");
        request.AuthorizeData = new AuthorizeData(Guid.Parse(userIdStr), userRoles,_httpContextAccessor.HttpContext.User.ToString(),endpoint);
        return await next(cancellationToken);
    }
}