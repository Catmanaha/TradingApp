namespace TradingApp.Repositories.Base;

public interface ICreate
{
    public Task<int> CreateAsync<T>(T model);
}
