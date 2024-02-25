using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IBidRepository : ICreate<Bid>
{
    public Task<Bid?> GetHighestBidForAuction(int auctionId);
    public Task<IEnumerable<object>> GetAllForAuction(int id);
}