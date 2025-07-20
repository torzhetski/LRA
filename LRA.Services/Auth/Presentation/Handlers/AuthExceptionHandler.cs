using LRA.Shared.Presentation.Handlers;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LRA.Auth.Presentation.Handlers;

public class AuthExceptionHandler : IExceptionHandler
{
    private readonly BaseExceptionHandler _baseExceptionHandler;

    public AuthExceptionHandler(BaseExceptionHandler baseExceptionHandler)
    {
        _baseExceptionHandler = baseExceptionHandler;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var handled = await _baseExceptionHandler.TryHandleAsync(httpContext, exception, cancellationToken);
        if (handled)
            return true;

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

        httpContext.Response.StatusCode = (int)response.Status!;
        var json = JsonSerializer.Serialize(response);
        await httpContext.Response.WriteAsync(json, cancellationToken);

        return true;
    }
}