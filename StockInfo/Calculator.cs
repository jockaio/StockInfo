using StockInfo.Entities;
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
                }
            }

            return result;
        }

        //Calculate if the owned Ricoschett Quotes should be sold or kept.
        public static void CalculateOwnedRicoschettQuotes(List<Quote> Quotes, int portfolioCode)
        {
            List<StockQuote> StockQuotes = null;
            List<StockQuote> SoldStockQuotes = new List<StockQuote>();
            Quote quote = null;
            using (StockDBContext db = new StockDBContext())
            {
                StockQuotes = Storage.ListOwnedStockQuotes(portfolioCode);

                if (StockQuotes != null && StockQuotes.Count > 0)
                {

                    foreach (var sq in StockQuotes)
                    {
                        quote = Quotes.Where(q => q.Symbol.ToLower().Equals(sq.Stock.Ticker.ToLower())).First();
                        if (decimal.Parse(quote.LastTradePriceOnly, CultureInfo.InvariantCulture) > sq.Price || ((DateTime.Today - sq.DateBought).TotalDays > 4))
                        {
                            StockQuote stockQuote = new StockQuote(quote)
                            {
                                DateBought = DateTime.Now,
                                Owned = false,
                                StrategyType = StrategyType.Ricoschett,
                                Quantity = sq.Quantity,
                                PortfolioCode = portfolioCode
                            };
                            stockQuote.Change = stockQuote.LastPrice - sq.Price;

                            Storage.SaveStockQuote(stockQuote);
                            SoldStockQuotes.Add(stockQuote);
                        }
                        else
                        {
                            db.StockQuotes.Where(s => s.ID == sq.ID).First().LastPrice = decimal.Parse(quote.LastTradePriceOnly, CultureInfo.InvariantCulture);
                            db.SaveChanges();
                        }
                        
                    }

                    if (SoldStockQuotes.Count > 0)
                    {
                        UpdatePortfolioFunds(SoldStockQuotes.Sum(s => s.Price * s.Quantity), 0, portfolioCode);
                    }
                    UpdatePortfolioInvestedValue(portfolioCode);
                }
            }
        }

        public static void AllocatePortfolioFunds(List<Quote> quotes, int portfolioCode)
        {
            if (quotes.Count > 0)
            {
                quotes.OrderByDescending(q => decimal.Parse(q.ChangeinPercent.Remove(q.ChangeinPercent.Count() - 1), CultureInfo.InvariantCulture)).ToList();

                List<StockQuote> sqs = new List<StockQuote>();

                decimal AvailableFunds = Storage.GetPortfolioFunds(portfolioCode) / quotes.Count;

                StockQuote sq = null;

                foreach (var q in quotes)
                {

                    sq = new StockQuote(q)
                    {
                        DateBought = DateTime.Now,
                        Owned = true,
                        StrategyType = StrategyType.Ricoschett,
                        PortfolioCode = portfolioCode,
                    };

                    if (sq.Price < AvailableFunds)
                    {
                        sq.Quantity = Convert.ToInt32(Math.Floor(AvailableFunds / sq.Price));
                        sqs.Add(sq);
                        //Buy the stock
                        Storage.SaveStockQuote(sq);
                    }
                }

                UpdatePortfolioFunds(0, sqs.Sum(s => s.Price * s.Quantity), portfolioCode);
                UpdatePortfolioInvestedValue(portfolioCode);
            }
        }

        public static void UpdatePortfolioFunds(decimal increase, decimal decrease, int portfolioCode)
        {
            if (increase > 0)
            {
                Storage.UpdatePortfolioFunds(39, Storage.GetPortfolioFunds(portfolioCode) + increase);
            }

            if (decrease > 0)
            {
                Storage.UpdatePortfolioFunds(39, Storage.GetPortfolioFunds(portfolioCode) - decrease);
            }
        }

        public static void UpdatePortfolioInvestedValue(int portfolioCode)
        {
            decimal investedValue = Storage.ListOwnedStockQuotes(portfolioCode).Sum(s => s.LastPrice * s.Quantity);
            Storage.UpdatePortfolioInvestedValue(39, investedValue);
        }
    }
}
