using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo
{
    public static class Calculator
    {
        public static List<Quote> CalculateRicoschettQuotes(List<Quote> Quotes)
        {
            List<Quote> result = new List<Quote>();
            decimal Range;
            decimal LastPriceInRange;

            foreach (var Quote in Quotes)
            {
                Range = decimal.Parse(Quote.DaysHigh, CultureInfo.InvariantCulture) - decimal.Parse(Quote.DaysLow, CultureInfo.InvariantCulture);
                LastPriceInRange = decimal.Parse(Quote.LastTradePriceOnly, CultureInfo.InvariantCulture) - decimal.Parse(Quote.DaysLow, CultureInfo.InvariantCulture);
                if (LastPriceInRange / Range < 0.1m)
                {
                    result.Add(Quote);
                    //Buy the stock
                    Storage.SaveStockQuote(new StockQuote(Quote)
                    {
                        DateBought = DateTime.Today,
                        Owned = true,
                        StrategyType = StrategyType.Ricoschett
                    });
                }
            }

            return result;
        }

        //Calculate if the owned Ricoschett Quotes should be sold or kept.
        public static void CalculateOwnedRicoschettQuotes(List<Quote> Quotes)
        {
            List<StockQuote> StockQuotes = null;
            Quote quote = null;
            using (StockDBContext db = new StockDBContext())
            {
                StockQuotes = db.StockQuotes.Where(s => s.Owned == true).ToList();

                foreach (var sq in StockQuotes)
                {
                    quote = Quotes.Where(q => q.Symbol.ToLower().Equals(sq.Stock.Ticker.ToLower())).First();
                    if (decimal.Parse(quote.LastTradePriceOnly, CultureInfo.InvariantCulture) > sq.Price || ((DateTime.Today - sq.DateBought).TotalDays > 4))
                    {
                        StockQuote stockQuote = new StockQuote(quote)
                        {
                            DateBought = DateTime.Today,
                            Owned = false,
                            StrategyType = StrategyType.Ricoschett
                        };
                        stockQuote.Change = stockQuote.Price - sq.Price;

                        Storage.SaveStockQuote(stockQuote);
                    }
                }
            }
        }
    }
}
