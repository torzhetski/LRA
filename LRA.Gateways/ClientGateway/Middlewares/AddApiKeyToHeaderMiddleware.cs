namespace LRA.Gateways.Client.Middlewares;

public class AddApiKeyToHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public AddApiKeyToHeaderMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = _config["InternalApiKey"];
        context.Request.Headers["X-Internal-Api-Key"] = apiKey;

        await _next(context);
    }
}
