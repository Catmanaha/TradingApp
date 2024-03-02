namespace TradingApp.Core.Repositories.Base;

public interface IGetById<T, TId>
{
    public Task<T?> GetByIdAsync(TId id);
}
