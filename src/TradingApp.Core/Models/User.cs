using Microsoft.AspNetCore.Identity;

namespace TradingApp.Core.Models;

public class User : IdentityUser<int>
{
    public double Balance { get; set; }
    public double StocksBalance { get; set; }
}