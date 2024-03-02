namespace TradingApp.Core.Repositories.Base;

public interface IGetAllForUser<T>
{
    public Task<IEnumerable<T>> GetAllForUserAsync(int id);
}
