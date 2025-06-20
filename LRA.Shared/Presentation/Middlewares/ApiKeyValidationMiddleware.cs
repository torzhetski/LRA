using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace LRA.Shared.Presentation.Middlewares;

public class ApiKeyValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _apiKeysWithRoles;

    public ApiKeyValidationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKeysWithRoles = configuration
            .GetSection("ApiKeys")
            .Get<List<string>>()
            ?.ToHashSet() ?? new();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("InternalApiKey", out var extractedApiKey) ||
            !_apiKeysWithRoles.Contains(extractedApiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized internal request");
            return;
        }

        var identity = new ClaimsIdentity(authenticationType: "InternalApiKey");
        context.User = new ClaimsPrincipal(identity);

        await _next(context);
    }
}
