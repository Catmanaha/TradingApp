using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class SellUserStockDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Count cannot be negative")]
    public double StockCount { get; set; }
    public int StockUuid { get; set; }
    public string StockName { get; set; }
}
