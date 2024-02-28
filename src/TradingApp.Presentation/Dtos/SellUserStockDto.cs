using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class SellUserStockDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Count cannot be negative")]
    public double StockCount { get; set; }
    public int UserStockId { get; set; }
    public string StockName { get; set; }
}
