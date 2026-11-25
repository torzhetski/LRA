using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LRA.Shared.Presentation.Handlers;

public class BaseExceptionHandler : IExceptionHandler
{
    private readonly ILogger<BaseExceptionHandler> _logger;

    public BaseExceptionHandler(ILogger<BaseExceptionHandler> logger)
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
                Status = StatusCodes.Status409Conflict
            },
            PermissionException unauthorizedEx => new ProblemDetails
            {
                Title = unauthorizedEx.Message,
                Status = StatusCodes.Status403Forbidden
            },
            UnauthorizedAccessException unauthorizedAccessEx => new ProblemDetails
            {
                Title = unauthorizedAccessEx.Message,
                Status = StatusCodes.Status401Unauthorized
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Title = notFoundEx.Message,
                Status = StatusCodes.Status404NotFound
            },
            UserBlockedException userBlockedEx => new ProblemDetails
            {
                Title = userBlockedEx.Message,
                Status = StatusCodes.Status423Locked
            },
            InvalidParametrException invalidParametrEx => new ProblemDetails
            {
                Title = invalidParametrEx.Message,
                Status = StatusCodes.Status400BadRequest
            },
            InvalidVerificationException InvalidVerificationEx => new ProblemDetails
            {
                Title = InvalidVerificationEx.Message,
                Status = StatusCodes.Status400BadRequest
            },
            _ => null!
        };

        if (response is null)
            return false;

        _logger.LogError(
            exception,
            "An error {ErrorDescription} occurred during processing the request {Path}",
            exception.Message,
            httpContext.Request.Path
        );

        httpContext.Response.StatusCode = response.Status ?? 500;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
