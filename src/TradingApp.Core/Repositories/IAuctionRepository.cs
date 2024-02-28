using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IAuctionRepository : ICreate<Auction>,
                                      IGetAll<Auction>,
                                      IGetById<Auction, int>,
                                      IUpdate<Auction>,
                                      IGetAllById<Auction>
{
    public Task<IEnumerable<Auction>> GetAllForUserAsync(int id);
}