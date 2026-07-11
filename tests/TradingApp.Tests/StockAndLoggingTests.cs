using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using TradingApp.Controllers;
using TradingApp.Middlewares;
using TradingApp.Models;
using TradingApp.Models.Managers;
using TradingApp.Repositories.Base;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Tests;

public class StockAndLoggingTests
{
    [Fact]
    public async Task StockCreationRejectsInvalidMarketCapWithoutCallingTheRepository()
    {
        var repository = new Mock<IStockRepository>();
        var controller = new StockController(repository.Object);

        var result = await controller.Create(new TradingApp.Dtos.StockDto
        {
            Symbol = "ABC",
            Name = "Example",
            MarketCap = -1
        });

        Assert.IsType<ViewResult>(result);
        repository.Verify(x => x.CreateAsync(It.IsAny<Stock>()), Times.Never);
    }

    [Fact]
    public async Task StockCreationPassesValidatedReferenceRecordsToTheRepository()
    {
        var repository = new Mock<IStockRepository>();
        repository.Setup(x => x.CreateAsync(It.IsAny<Stock>())).ReturnsAsync(1);
        var controller = new StockController(repository.Object);

        var result = await controller.Create(new TradingApp.Dtos.StockDto
        {
            Symbol = "ABC",
            Name = "Example",
            MarketCap = 100
        });

        Assert.IsType<RedirectToActionResult>(result);
        repository.Verify(x => x.CreateAsync(It.Is<Stock>(stock =>
            stock.Symbol == "ABC" && stock.Name == "Example" && stock.MarketCap == 100)), Times.Once);
    }

    [Fact]
    public async Task LoggingPersistsMetadataWithoutQueryStringsOrBodies()
    {
        Log? captured = null;
        var repository = new Mock<ILogRepository>();
        repository.Setup(x => x.CreateAsync(It.IsAny<Log>()))
            .Callback<Log>(log => captured = log)
            .ReturnsAsync(1);
        var middleware = new LogMiddleware(repository.Object, new StaticOptionsMonitor<LogManager>(new LogManager { IsLoggerEnabled = true }));
        var context = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection().BuildServiceProvider()
        };
        context.Request.Path = "/User/Login";
        context.Request.QueryString = new QueryString("?password=secret");
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "7")
        }));

        await middleware.InvokeAsync(context, next: httpContext =>
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        });

        Assert.NotNull(captured);
        Assert.Equal(7, captured!.UserId);
        Assert.Equal("/User/Login", captured.Url);
        Assert.DoesNotContain("password", captured.Url, StringComparison.OrdinalIgnoreCase);
        Assert.Null(captured.RequestBody);
        Assert.Null(captured.ResponseBody);
    }

    private sealed class StaticOptionsMonitor<T>(T value) : IOptionsMonitor<T>
    {
        public T CurrentValue { get; } = value;
        public T Get(string? name) => CurrentValue;
        public IDisposable OnChange(Action<T, string?> listener) => NoopDisposable.Instance;

        private sealed class NoopDisposable : IDisposable
        {
            public static readonly NoopDisposable Instance = new();
            public void Dispose() { }
        }
    }
}
