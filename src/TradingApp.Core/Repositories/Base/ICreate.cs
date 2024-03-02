namespace TradingApp.Core.Repositories.Base;

public interface ICreate<T>
{
    public Task<T> CreateAsync(T model);
}
