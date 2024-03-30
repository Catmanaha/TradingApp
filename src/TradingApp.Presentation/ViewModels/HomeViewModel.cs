using TradingApp.Core.Models;
using TradingApp.Core.Models.Stocks;

namespace TradingApp.Presentation.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Stock> Stocks { get; set; }
    public IEnumerable<Article> Articles { get; set; }
}
