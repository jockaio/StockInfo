using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo.Entities
{
    public class Portfolio
    {
        public int ID { get; set; }
        public int PortfolioCode { get; set; }
        public decimal Funds { get; set; }
        public decimal InvestedValue { get; set; }
        public DateTime Date { get; set; }
    }
}
