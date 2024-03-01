using TradingApp.Core.Dtos;
using TradingApp.Core.Enums;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;

namespace TradingApp.Core.Services;

public interface IAuctionService
{
    public Task<IEnumerable<AuctionForUser>> GetAllForUser(int id);
    public Task<IEnumerable<AuctionForUser>> GetAllForView();
    public Task ChangeStatus(AuctionStatusEnum status, int id);
    public Task Sell(SellAuctionDto dto);
    public Task<Auction> GetById(int id);
    public Task<Auction> CreateAsync(SellAuctionDto dto);
}
