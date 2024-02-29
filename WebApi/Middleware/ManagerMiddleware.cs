using System.Net;
using Newtonsoft.Json;

namespace WebApi.Middleware;

public class ManagerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManagerMiddleware> _logger;

    public ManagerMiddleware(
        RequestDelegate next,
        ILogger<ManagerMiddleware?> logger
    )
    {
        _next = next;
        _logger = logger!;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await ManagerExceptionAsync(context, e, _logger);
        }
    }

    private static async Task ManagerExceptionAsync(
        HttpContext context,
        Exception ex,
        ILogger<ManagerMiddleware> logger
    )
    {
        string message = string.Empty;

        switch (ex)
        {
            case MiddlewareException me:
                logger.LogError(ex, "Middleware Error");
                message = me.Error;
                context.Response.StatusCode = (int)me.Code;
                break;
            case Exception e:
                logger.LogError(e, "Server Error");
                message = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";
        string results = message != null ? JsonConvert.SerializeObject(new { message }) : string.Empty;

        await context.Response.WriteAsync(results);
    }
}