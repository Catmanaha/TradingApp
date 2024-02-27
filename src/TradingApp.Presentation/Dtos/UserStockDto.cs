using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class UserStockDto
{
    public int UserId { get; set; }

    public string StockUuid { get; set; }

    [Required(ErrorMessage = "StockCount cannot be empty")]
    [Range(0, double.MaxValue, ErrorMessage = "Count cannot be negative")]
    public double StockCount { get; set; }

    public double StockPrice { get; set; }
    public string StockName { get; set; }
}
