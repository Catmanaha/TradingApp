using TradingApp.Core.Dtos;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;

namespace TradingApp.Core.Services;

public interface IBidService
{
    public Task<IEnumerable<BidForAuction>> GetAllForAuction(int id);
    public Task Bid(BidDto dto);
    public Task CreateAsync(BidDto dto, User user);
}
