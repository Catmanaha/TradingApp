namespace TradingApp.Repositories.Base;

public interface IGetAll<T>
{
    public Task<IEnumerable<T>> GetAllAsync();
}
