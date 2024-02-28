using TradingApp.Core.Enums;
using TradingApp.Core.Models.ReturnsForServices;

namespace TradingApp.Core.Services;

public interface IAuctionService
{
    public Task<IEnumerable<AuctionForUser>> GetAllForUser(int id);
    public Task<IEnumerable<AuctionForUser>> GetAllForView();
    public Task ChangeStatus(AuctionStatusEnum status, int id);
}
