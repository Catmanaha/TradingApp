using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Core.Models
{
    public class News
    {
        public IEnumerable<Article>? Articles { get; set; }
        public int Offset { get; set; }
    }
}