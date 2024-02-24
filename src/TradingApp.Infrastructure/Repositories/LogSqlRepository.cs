using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;

namespace TradingApp.Infrastructure.Repositories;

public class LogSqlRepository : ILogRepository
{
    private readonly TradingAppDbContext DBC;

    public LogSqlRepository(TradingAppDbContext DBC)
    {
        this.DBC = DBC;
    }

    public async Task CreateAsync(Log log)
    {
        await DBC.Logs.AddAsync(log);
        await DBC.SaveChangesAsync();
    }
}