using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using TradingApp.Models;
using TradingApp.Models.Managers;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Middlewares;

public class LogMiddleware : IMiddleware
{
    private readonly ILogRepository repository;
    private readonly IOptionsMonitor<LogManager> optionsMonitor;
    private readonly IDataProtector dataProtector;

    public LogMiddleware(ILogRepository repository, IDataProtectionProvider dataProtectionProvider, IOptionsMonitor<LogManager> optionsMonitor)
    {
        this.repository = repository;
        this.optionsMonitor = optionsMonitor;
        this.dataProtector = dataProtectionProvider.CreateProtector("IdentityProtection");
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (optionsMonitor.CurrentValue.IsLoggerEnabled == false) {
            await next.Invoke(context);
            return;
        }

        int userId = default;
        if (context.Request.Cookies["UserId"] is not null)
        {
            userId = int.Parse(dataProtector.Unprotect(context.Request.Cookies["UserId"]!));
        }

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
