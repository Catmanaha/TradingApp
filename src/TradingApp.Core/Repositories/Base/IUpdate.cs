using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Core.Repositories.Base
{
    public interface IUpdate<T>
    {
        public Task UpdateAsync(T model);
    }
}