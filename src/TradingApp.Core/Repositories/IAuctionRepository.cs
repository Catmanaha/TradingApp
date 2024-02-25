using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IAuctionRepository : ICreate<Auction>, IGetAllForUser<object>, IGetAll<object>, IGetById<Auction>, IUpdate<Auction> { }