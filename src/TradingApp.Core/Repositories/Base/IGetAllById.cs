namespace TradingApp.Core.Repositories.Base;

public interface IGetAllById<T>
{
    public Task<IEnumerable<T>> GetAllByIdAsync(int id);
}
