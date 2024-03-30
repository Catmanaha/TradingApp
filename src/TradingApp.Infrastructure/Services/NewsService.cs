using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository newsRepository;

    public NewsService(INewsRepository newsRepository)
    {
        this.newsRepository = newsRepository;
    }

    public async Task<News> GetAll(int offset = 0) {
        return await newsRepository.GetNews(offset);
    }
}
