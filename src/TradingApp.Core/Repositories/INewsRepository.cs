using TradingApp.Core.Models;

namespace TradingApp.Core.Repositories;

public interface INewsRepository
{
    public Task<News> GetNews(int offset = 0);
}
