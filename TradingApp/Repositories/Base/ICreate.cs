namespace TradingApp.Repositories.Base;

public interface ICreate<T>
{
    public Task<int> CreateAsync(T model);
}
