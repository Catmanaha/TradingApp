namespace TradingApp.Core.Repositories.Base;

public interface ICreate<T>
{
    public Task CreateAsync(T model);
}
