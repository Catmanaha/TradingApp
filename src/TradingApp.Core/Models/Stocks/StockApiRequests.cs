namespace TradingApp.Core.Models.Stocks;

public static class StockApiRequests
{
    public static string GetAll(int limit, int offset) => $"coins?limit={limit}&offset={offset}";
    public static string GetAll() => $"coins";
    public static string GetById(string uuid) => $"coin/{uuid}";
    public static string GetById(string uuid, string timePeriod) => $"coin/{uuid}?timePeriod={timePeriod}";
    public static string StockPriceHistory(string uuid, string timePeriod) => $"coin/{uuid}/history?timePeriod={timePeriod}";
    public static string StockPriceHistory(string uuid) => $"coin/{uuid}/history";
    public static string GetStockOHLC(string uuid) => $"coin/{uuid}/ohlc";
}
