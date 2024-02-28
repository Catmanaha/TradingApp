namespace TradingApp.Core.Repositories.Base;

public interface IUpdate<T>
{
    public Task UpdateAsync(T model);
}
