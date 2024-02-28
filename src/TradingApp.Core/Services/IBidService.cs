using TradingApp.Core.Models.ReturnsForServices;

namespace TradingApp.Core.Services;

public interface IBidService
{
    public Task<IEnumerable<BidForAuction>> GetAllForAuction(int id);
}
