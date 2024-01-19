using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using TradingApp.Dtos;
using TradingApp.Models;
using TradingApp.Repositories.Base;

namespace TradingApp.Repositories;

public class StockSqlRepository : ISqlRepository<Stock>
{
    private readonly SqlConnection connection;

    public StockSqlRepository(SqlConnection connection)
    {
        this.connection = connection;
    }

    public async Task<int> CreateAsync(Stock stock)
    {
        return await connection.ExecuteAsync(@"insert into Stocks(Symbol, Name, MarketCap) 
                                               values(@Symbol, @Name, @MarketCap)", stock);
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await connection.QueryAsync<Stock>("select * from Stocks");
    }
}