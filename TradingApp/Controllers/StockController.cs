using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using TradingApp.Attributes.Http;
using TradingApp.Controllers.Base;
using Dapper;
using TradingApp.Extensions;
using TradingApp.Models;
using TradingApp.Models.Base;

namespace TradingApp.Controllers;

public class StockController : ControllerBase
{
    private const string connectionString = "Server=localhost;Database=TradingAppDb;User Id=sa;Password=Tomik2008;";

    [HttpGet("GetAll")]
    public async Task<ActionResult> GetAll()
    {
        using var connection = new SqlConnection(connectionString);
        var stocks = await connection.QueryAsync<Stock>("select * from Stocks");

        var stocksHtml = stocks.GetHtml();

        return ObjectView(stocksHtml);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult> GetStockById()
    {
        var stockIdToGetObj = base.HttpContext.Request.QueryString["id"];

        if (stockIdToGetObj == null)
        {
            return BadRequest("Didnt send stock id");
        }

        if (int.TryParse(stockIdToGetObj, out int stockIdToGet) == false)
        {
            return BadRequest("id isnt integer");
        }

        using var connection = new SqlConnection(connectionString);
        var stock = await connection.QueryFirstOrDefaultAsync<Stock>(
            sql: "select top 1 * from Stocks where Id = @Id",
            param: new { Id = stockIdToGet });

        if (stock is null)
        {
            return NotFound();
        }

        return Ok(stock);
    }

    [HttpGet("GetByName")]
    public async Task<ActionResult> GetStockByName()
    {
        var stockNameToGetObj = base.HttpContext.Request.QueryString["name"];

        if (stockNameToGetObj == null)
        {
            return BadRequest("didnt send name");
        }

        using var connection = new SqlConnection(connectionString);
        var stocks = await connection.QueryAsync<Stock>(
            sql: $"select * from Stocks where Name like @NameLike",
            new { NameLike = "%" + stockNameToGetObj + "%" }
        );

        if (stocks is null)
        {
            return NotFound();
        }

        var stockPage = stocks.GetHtml();

        return ObjectView(stockPage);
    }

    [HttpPost]
    public async Task<ActionResult> Create()
    {
        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        if (json == "{}")
        {
            return BadRequest("Send stock json");
        }

        var newStock = JsonSerializer.Deserialize<Stock>(json);

        if (string.IsNullOrWhiteSpace(newStock.MarketCap))
        {
            return BadRequest("MarketCap is empty");
        }

        if (string.IsNullOrWhiteSpace(newStock.Name))
        {
            return BadRequest("Name is empty");
        }

        if (string.IsNullOrWhiteSpace(newStock.Symbol))
        {
            return BadRequest("Symbol is empty");
        }

        using var connection = new SqlConnection(connectionString);
        var stocks = await connection.ExecuteAsync(
            @"insert into Stocks (Symbol, Name, MarketCap) 
        values(@Symbol, @Name, @MarketCap)",
            param: newStock);

        return Created();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete()
    {
        var stockIdToDeleteObj = base.HttpContext.Request.QueryString["id"];
        System.Console.WriteLine(stockIdToDeleteObj);
        if (stockIdToDeleteObj == null)
        {
            return BadRequest("Didnt send stock id");
        }

        if (int.TryParse(stockIdToDeleteObj, out int stockIdToDelete) == false)
        {
            return BadRequest("id isnt integer");
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
            return NotFound();
        }

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Edit()
    {
        var stockIdToUpdateObj = base.HttpContext.Request.QueryString["id"];

        if (stockIdToUpdateObj == null)
        {
            return BadRequest("Didnt send stock id");
        }

        if (int.TryParse(stockIdToUpdateObj, out int stockIdToDelete) == false)
        {
            return BadRequest("id isnt integer");
        }

        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        if (json == "{}")
        {
            return BadRequest("Send stock json");
        }

        var stockToUpdate = JsonSerializer.Deserialize<Stock>(json);

        if (string.IsNullOrWhiteSpace(stockToUpdate.MarketCap))
        {
            return BadRequest("MarketCap is empty");
        }

        if (string.IsNullOrWhiteSpace(stockToUpdate.Name))
        {
            return BadRequest("Name is empty");
        }

        if (string.IsNullOrWhiteSpace(stockToUpdate.Symbol))
        {
            return BadRequest("Symbol is empty");
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
                Id = stockIdToUpdateObj
            });

        if (affectedRowsCount == 0)
        {
            return NotFound();
        }

        return Ok();
    }
}