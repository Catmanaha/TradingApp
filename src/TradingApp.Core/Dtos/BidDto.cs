using System.ComponentModel.DataAnnotations;

namespace TradingApp.Core.Dtos;

public class BidDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage ="Bid amount cannot be negative")]
    public double BidAmount { get; set; }
    [Required]
    public int AuctionId { get; set; }
}
