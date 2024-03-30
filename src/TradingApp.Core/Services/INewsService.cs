using TradingApp.Core.Models;

namespace TradingApp.Core.Services;

public interface INewsService
{
    public Task<News> GetAll(int offset = 0);
}
