using System.ComponentModel.DataAnnotations;

namespace TradingApp.Core.Dtos;

public class CashInDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Amount cannot be negative")]
    public double AmoutToAdd { get; set; }
}
