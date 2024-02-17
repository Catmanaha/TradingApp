namespace TradingApp.Dtos;

public class UserStockDto
{
    public int UserId { get; set; }
    public int StockId { get; set; }
    public string StockName { get; set; }
    public int StockCount { get; set; }
}
