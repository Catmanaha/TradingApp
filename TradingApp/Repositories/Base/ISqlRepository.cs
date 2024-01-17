using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Repositories.Base;

public interface ISqlRepository<T>
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<int> CreateAsync(T model);
    public Task<T?> GetByIdAsync(int id);
}
