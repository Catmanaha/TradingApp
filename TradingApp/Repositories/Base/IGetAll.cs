using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Repositories.Base;

public interface IGetAll<T>
{
    public Task<IEnumerable<T>> GetAllAsync();
}
