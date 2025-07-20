using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _ => null!
        };

        if (response is null)
            return false;

        httpContext.Response.StatusCode = response.Status ?? 500;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
