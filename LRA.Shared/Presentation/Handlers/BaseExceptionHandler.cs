using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Shared.Presentation.Handlers;

public class BaseExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";

        var response = exception switch
        {
            AlreadyExistsException alreadyExsistseEx => new ProblemDetails
            {
                Title = alreadyExsistseEx.Message,
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
                Status = StatusCodes.Status400BadRequest
            },
            _ => null!
        };

        if (response is null)
            return false;

        httpContext.Response.StatusCode = response.Status ?? 500;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
