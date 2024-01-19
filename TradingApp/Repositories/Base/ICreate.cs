using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Repositories.Base;

public interface ICreate<T>
{
    public Task<int> CreateAsync(T model);
}
