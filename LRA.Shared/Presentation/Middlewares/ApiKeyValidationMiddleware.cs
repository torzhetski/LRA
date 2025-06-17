using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace LRA.Shared.Presentation.Middlewares;

public class ApiKeyValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string, string> _apiKeysWithRoles;

    public ApiKeyValidationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKeysWithRoles = configuration
            .GetSection("ApiKeys")
            .Get<List<ApiKeyEntry>>()
            ?.ToDictionary(x => x.Key, x => x.Role) ?? new();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("InternalApiKey", out var extractedApiKey) ||
            !_apiKeysWithRoles.TryGetValue(extractedApiKey, out var role))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized internal request");
            return;
        }

        var claims = new[] { new Claim(ClaimTypes.Role, role) };
        var identity = new ClaimsIdentity(claims, "InternalApiKey");
        context.User = new ClaimsPrincipal(identity);

        await _next(context);
    }

    private class ApiKeyEntry
    {
        public string Key { get; set; }
        public string Role { get; set; }
    }
}
