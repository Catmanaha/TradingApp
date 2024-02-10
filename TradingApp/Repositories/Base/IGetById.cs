namespace TradingApp.Repositories.Base;
public interface IGetById<T>
{
    public Task<T> GetByIdAsync(int id);
}
