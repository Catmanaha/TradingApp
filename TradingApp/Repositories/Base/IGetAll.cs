namespace TradingApp.Repositories.Base;

public interface IGetAll
{
    public Task<IEnumerable<T>> GetAllAsync<T>();
}
