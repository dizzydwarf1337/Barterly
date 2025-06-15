using API.Core.ApiResponse;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.BusinessExceptions;
using Application.Core.Validators.Interfaces;
using Microsoft.Extensions.Logging;
using Domain.Entities.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Core.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>

     where TRequest : notnull
    {
        private readonly IAuthChecker _authChecker;
        private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizationBehavior(IAuthChecker authChecker, ILogger<AuthorizationBehavior<TRequest,TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _authChecker = authChecker;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Entering authorization behavior for request type {0}", typeof(TRequest).Name);
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (request is IPostOwner postOwner)
            {
                await _authChecker.CheckPostPermission(userIdStr);
            }
            else if (request is IOpinionOwner opinionOwner)
            {
                await _authChecker.CheckCommentPermission(userIdStr);
            }
            return await next(cancellationToken);
        }
    }

}
