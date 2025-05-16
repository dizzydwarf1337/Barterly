using API.Core.ApiResponse;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Application.Core.Validators.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Application.DTOs.Posts;

public class ValidationFilter : IActionFilter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOwnershipValidator _ownershipValidator;

    public ValidationFilter(IServiceProvider serviceProvider, IOwnershipValidator ownershipValidator)
    {
        _serviceProvider = serviceProvider;
        _ownershipValidator = ownershipValidator;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        var hasAuthorize = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() != null;

        Guid userId = Guid.Empty;
        if (!hasAuthorize)
        {
            return;
        }
        var userIdStr = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        foreach (var argument in context.ActionArguments)
        {
            var value = argument.Value;
            Console.WriteLine(argument);
            Console.WriteLine(value);
            if (value == null) continue;

            if (argument.Key.Equals("post", StringComparison.OrdinalIgnoreCase) && value is CreatePostDto postDto)
            {
                var ownerIdFromDto = Guid.Parse(postDto.OwnerId);
                try
                {
                    _ownershipValidator.ValidateAccountOwnership(userId, ownerIdFromDto).Wait();
                }
                catch (Exception ex)
                {
                    context.Result = Forbidden(ex);
                    return;
                }

            }
            else if (argument.Key.Equals("post", StringComparison.OrdinalIgnoreCase) && value is EditPostDto editPostDto)
            {
                try
                {
                    _ownershipValidator.ValidatePostOwnership(userId, Guid.Parse(editPostDto.Id)).Wait();
                }
                catch (Exception ex)
                {
                    context.Result = Forbidden(ex);
                    return;
                }
            }
            else if (argument.Key.Equals("imagesDto", StringComparison.OrdinalIgnoreCase) && value is ImagesDto imagesDto)
            {
                try
                {
                    _ownershipValidator.ValidatePostOwnership(userId, Guid.Parse(imagesDto.PostId)).Wait();
                }
                catch (Exception ex)
                {
                    context.Result = Forbidden(ex);
                    return;
                }
            }
            else if (argument.Key.Equals("postId", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    _ownershipValidator.ValidatePostOwnership(userId, Guid.Parse((string)value)).Wait();
                }
                catch (Exception ex)
                {
                    context.Result = Forbidden(ex);
                    return;
                }
            }
        }
    }

    private ObjectResult Forbidden(Exception ex)
    {
        var response = ApiResponse<string>.Failure(ex.Message, 403);
        return new ObjectResult(response) { StatusCode = 403 };
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
