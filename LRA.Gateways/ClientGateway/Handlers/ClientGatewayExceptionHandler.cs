using LRA.Shared.Presentation.Handlers;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LRA.Gateways.Client.Handlers;

public class ClientGatewayExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ClientGatewayExceptionHandler> _logger;

    public ClientGatewayExceptionHandler(ILogger<ClientGatewayExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";

        var response = exception switch
        {
            AlreadyExistsException alreadyExistsEx => new ProblemDetails
            {
                Title = alreadyExistsEx.Message,
                Status = StatusCodes.Status400BadRequest
            },
            UnauthorizedAccessException unauthorizedAccessEx => new ProblemDetails
            {
                Title = unauthorizedAccessEx.Message,
                Status = StatusCodes.Status401Unauthorized
            },
            UserBlockedException userBlockedEx => new ProblemDetails
            {
                Title = userBlockedEx.Message,
                Status = StatusCodes.Status400BadRequest
            },
            InvalidParametrException invalidParametrEx => new ProblemDetails
            {
                Title = invalidParametrEx.Message,
                Status = StatusCodes.Status400BadRequest
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Title = notFoundEx.Message,
                Status = StatusCodes.Status404NotFound
            },
            InvalidVerificationException InvalidVerificationEx => new ProblemDetails
            {
                Title = InvalidVerificationEx.Message,
                Status = StatusCodes.Status400BadRequest
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Title = "Internal Server Error",
            }
        };

        _logger.LogError(
            exception,
            "An error {ErrorDescription} occurred during processing the request {Path}",
            exception.Message,
            httpContext.Request.Path
        );

        httpContext.Response.StatusCode = (int)response.Status!;
        var json = JsonSerializer.Serialize(response);
        await httpContext.Response.WriteAsync(json, cancellationToken);

        return true;
    }
}
