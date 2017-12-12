using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo.Entities
{
    class Portfolio
    {
        public decimal Funds { get; set; }
        public List<Stock> OwnedStocks { get; set; }
    }
}
