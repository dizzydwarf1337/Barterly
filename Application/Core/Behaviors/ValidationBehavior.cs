using API.Core.ApiResponse;
using Application.Core.Validators.Interfaces;
using Application.DTOs.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Behaviors
{
    using API.Core.ApiResponse;
    using Application.Core.Validators.Interfaces;
    using Application.Features.Posts.Commands.CreatePost;
    using Application.Interfaces.CommandInterfaces;
    using Domain.Exceptions.BusinessExceptions;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull

    {
        private readonly IAccountOwnershipValidator _accountValidator;
        private readonly IOpinionOwnershipValidator _opinionOwnershipValidator;
        private readonly IPostOwnershipValidator _postOwnershipValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IAccountOwnershipValidator accountValidator,
            IOpinionOwnershipValidator opinionOwnershipValidator,
            IPostOwnershipValidator postOwnershipValidator,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ValidationBehavior<TRequest,TResponse>> logger)
        {
            _accountValidator = accountValidator;
            _opinionOwnershipValidator = opinionOwnershipValidator;
            _postOwnershipValidator = postOwnershipValidator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var endpoint = _httpContextAccessor.HttpContext?.GetEndpoint();
            var hasAuthorize = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() != null;
            if (!hasAuthorize)
            {
                return await next();
            }
            _logger.LogDebug("Entering vlidation behavior for request type {0} and user {1}", typeof(TRequest).Name, userIdStr);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var requestType = request.GetType();

            if (request is IHasOwner hasOwner)
            {
                await _accountValidator.ValidateAccountOwnership(userId, Guid.Parse(hasOwner.OwnerId));
            }
            if (request is IPostOwner postOwner)
            {
                await _postOwnershipValidator.ValidatePostOwnership(userId, Guid.Parse(postOwner.PostId));
            }
            if(request is IOpinionOwner opinionOwner)
            {
                await _opinionOwnershipValidator.ValidateOpinionOwnership(userId, Guid.Parse(opinionOwner.OpinionId));
            }
            return await next();
        }
    }


}
