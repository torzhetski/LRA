using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LRA.Account.Presentation.Handlers;

public class AccountExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AccountExceptionHandler> _logger;

    public AccountExceptionHandler(ILogger<AccountExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";

        var response = exception switch
        {
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
