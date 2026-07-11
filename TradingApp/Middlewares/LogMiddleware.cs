using Microsoft.Extensions.Options;
using TradingApp.Models;
using TradingApp.Models.Managers;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Middlewares;

public class LogMiddleware : IMiddleware
{
    private readonly ILogRepository repository;
    private readonly IOptionsMonitor<LogManager> optionsMonitor;

    public LogMiddleware(ILogRepository repository, IOptionsMonitor<LogManager> optionsMonitor)
    {
        this.repository = repository;
        this.optionsMonitor = optionsMonitor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (optionsMonitor.CurrentValue.IsLoggerEnabled == false)
        {
            await next.Invoke(context);
            return;
        }

        await next.Invoke(context);

        await repository.CreateAsync(new Log
        {
            UserId = TryGetUserId(context),
            Url = context.Request.Path,
            MethodType = context.Request.Method,
            StatusCode = context.Response.StatusCode,
            RequestBody = null,
            ResponseBody = null,
        });
    }

    private static int TryGetUserId(HttpContext context)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return default;
        }

        return int.TryParse(userId, out var parsedUserId) ? parsedUserId : default;
    }
}
