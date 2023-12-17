using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using TradingApp.Attributes.Http;
using TradingApp.Controllers.Base;
using Dapper;
using TradingApp.Extensions;
using TradingApp.Models;

namespace TradingApp.Controllers;

public class StockController : ControllerBase
{
    private const string connectionString = "Server=localhost;Database=TradingAppDb;User Id=sa;Password=Tomik2008;";

    [HttpGet("GetAll")]
    public async Task GetStocksAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);

        using var connection = new SqlConnection(connectionString);
        var stocks = await connection.QueryAsync<Stock>("select * from Stocks");

        var stocksHtml = stocks.GetHtml();
        await writer.WriteLineAsync(stocksHtml);
        context.Response.ContentType = "text/html";

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetStockByIdAsync(HttpListenerContext context)
    {
        var stockIdToGetObj = context.Request.QueryString["id"];

        if (stockIdToGetObj == null || int.TryParse(stockIdToGetObj, out int stockIdToGet) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(connectionString);
        var stock = await connection.QueryFirstOrDefaultAsync<Stock>(
            sql: "select top 1 * from Stocks where Id = @Id",
            param: new { Id = stockIdToGet });

        if (stock is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(stock));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("SearchByName")]
    public async Task GetStockByNameAsync(HttpListenerContext context)
    {
        var stockNameToGetObj = context.Request.QueryString["name"];

        if (stockNameToGetObj == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(connectionString);
        var stocks = await connection.QueryAsync<Stock>(
            sql: $"select * from Stocks where Name like @NameLike",
            new { NameLike = "%" + stockNameToGetObj + "%" }
        );

        if (stocks is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(stocks));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPost("Create")]
    public async Task PostStockAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        if (json == "{}")
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("Send stock json");
            return;
        }

        var newStock = JsonSerializer.Deserialize<Stock>(json);

        if (string.IsNullOrWhiteSpace(newStock.MarketCap))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("MarketCap is empty");
            return;
        }

        if (string.IsNullOrWhiteSpace(newStock.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("Name is empty");
            return;
        }

        if (string.IsNullOrWhiteSpace(newStock.Symbol))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("Symbol is empty");
            return;
        }

        using var connection = new SqlConnection(connectionString);
        var stocks = await connection.ExecuteAsync(
            @"insert into Stocks (Symbol, Name, MarketCap) 
        values(@Symbol, @Name, @MarketCap)",
            param: newStock);

        context.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteStockAsync(HttpListenerContext context)
    {
        var stockIdToDeleteObj = context.Request.QueryString["id"];

        if (stockIdToDeleteObj == null || int.TryParse(stockIdToDeleteObj, out int stockIdToDelete) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(connectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Stocks
        where Id = @Id",
            param: new
            {
                Id = stockIdToDelete,
            });

        if (deletedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutStockAsync(HttpListenerContext context)
    {
        var stockIdToUpdateObj = context.Request.QueryString["id"];

        if (stockIdToUpdateObj == null || int.TryParse(stockIdToUpdateObj, out int stockIdToUpdate) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        if (json == "{}")
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("Send stock json");
            return;
        }

        var stockToUpdate = JsonSerializer.Deserialize<Stock>(json);

        if (string.IsNullOrWhiteSpace(stockToUpdate.MarketCap))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("MarketCap is empty");
            return;
        }

        if (string.IsNullOrWhiteSpace(stockToUpdate.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("Name is empty");
            return;
        }

        if (string.IsNullOrWhiteSpace(stockToUpdate.Symbol))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteAsync("Symbol is empty");
            return;
        }

        using var connection = new SqlConnection(connectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Stocks
        set Symbol = @Symbol, Name = @Name, MarketCap = @MarketCap
        where Id = @Id",
            param: new
            {
                stockToUpdate.Symbol,
                stockToUpdate.Name,
                stockToUpdate.MarketCap,
                Id = stockIdToUpdate
            });

        if (affectedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}