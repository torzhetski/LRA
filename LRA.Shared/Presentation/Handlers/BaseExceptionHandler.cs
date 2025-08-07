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
