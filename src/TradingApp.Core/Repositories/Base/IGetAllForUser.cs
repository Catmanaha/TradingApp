namespace TradingApp.Core.Repositories.Base;

public interface IGetAllForUser<T>
{
    public Task<IEnumerable<T>> GetAllForUser(int id);
}
