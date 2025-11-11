using Application.Core.ApiResponse;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Domain.Exceptions.ExternalServicesExceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning("Entering exception handling behavior for request type {0}", typeof(TRequest));
        
        _logger.LogWarning("TResponse{0}", typeof(TResponse));

        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in pipeline");

            var response = CreateErrorResponse(ex);
            return response ?? throw new InvalidOperationException("Failed to create error response", ex);
        }
    }

    private TResponse? CreateErrorResponse(Exception ex)
    {
        if (!typeof(TResponse).IsGenericType ||
            typeof(TResponse).GetGenericTypeDefinition() != typeof(ApiResponse<>))
            return default;

        dynamic response = Activator.CreateInstance(typeof(TResponse))!;

        response.IsSuccess = false;
        response.StatusCode = ex switch
        {
            EntityNotFoundException => 404,
            ConfigException => 400,
            AccessForbiddenException => 401,
            EntityAlreadyExistsException => 409,
            EntityCreatingException => 500,
            InvalidDataProvidedException => 400,
            ExternalServiceException => 502,
            _ => 500
        };

        response.Error = ex.Message;

        return (TResponse)response;
    }
}