using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TradingApp.Core.Models;
using TradingApp.Core.Models.Configurations;
using TradingApp.Core.Repositories;

namespace TradingApp.Presentation.Middlewares;

public class LogMiddleware : IMiddleware
{
    private readonly ILogRepository repository;
    private readonly LogConfiguration logManager;
    private readonly UserManager<User> userManager;

    public LogMiddleware(
        ILogRepository repository,
        IOptionsSnapshot<LogConfiguration> optionsSnapshot,
        UserManager<User> userManager
    )
    {
        this.repository = repository;
        this.logManager = optionsSnapshot.Value;
        this.userManager = userManager;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (logManager.IsLoggerEnabled == false)
        {
            await next.Invoke(context);
            return;
        }

        int userId = default;
        int.TryParse(userManager.GetUserId(context.User), out userId);

        context.Request.EnableBuffering();
        var requestRead = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var originalResponseBody = context.Response.Body;

        using var memoryStream = new MemoryStream();

        context.Response.Body = memoryStream;

        await next.Invoke(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseRead = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        await context.Response.Body.CopyToAsync(originalResponseBody);

        await repository.CreateAsync(new Log
        {
            UserId = userId,
            Url = context.Request.GetDisplayUrl(),
            MethodType = context.Request.Method,
            StatusCode = context.Response.StatusCode,
            RequestBody = requestRead,
            ResponseBody = responseRead
        });

    }
}
