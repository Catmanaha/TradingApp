using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;

namespace TradingApp.Infrastructure.Data;

public class TradingAppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Log> Logs { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<UserStock> UserStocks { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }

    public TradingAppDbContext(DbContextOptions<TradingAppDbContext> options) : base(options) {}
}
