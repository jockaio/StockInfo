using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace StockInfo.Entities
{
    public class StockQuote
    {
        public DateTime Created { get; set; }
        public int ID { get; set; }
        public int StockID { get; set; }
        [ForeignKey("StockID")]
        public virtual Stock Stock { get; set; }
        public decimal Price { get; set; }
        public decimal LastPrice { get; set; }
        public bool Owned { get; set; }
        public DateTime DateBought { get; set; }
        public StrategyType StrategyType { get; set; }
        public decimal? Change { get; set; }
        public int Quantity { get; set; }
        public int PortfolioCode { get; set; }

        public StockQuote() { }

        public StockQuote(Quote quote)
        {
            Created = DateTime.Now;
            Price = decimal.Parse(quote.LastTradePriceOnly, CultureInfo.InvariantCulture);
            LastPrice = decimal.Parse(quote.LastTradePriceOnly, CultureInfo.InvariantCulture);
            using (StockDBContext db = new StockDBContext())
            {
                Stock = db.Stocks.Where(s => s.Ticker == quote.Symbol).First();
            }

            StockID = Stock.ID;
            
        }
    }
}