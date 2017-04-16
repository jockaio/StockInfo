using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo
{
    public static class Storage
    {
        public static void SaveStockQuote(StockQuote stockQuote)
        {
            using (StockDBContext db = new StockDBContext())
            {
                if (!StockQuoteExists(stockQuote))
                {
                    db.Stocks.Attach(stockQuote.Stock);
                    db.Entry(stockQuote.Stock).State = System.Data.Entity.EntityState.Unchanged;
                    db.StockQuotes.Add(stockQuote);
                    db.SaveChanges();
                }
            }
        }

        public static List<StockQuote> ListOwnedStockQuotes(int portfolioCode)
        {
            List<StockQuote> result = new List<StockQuote>();
            using (StockDBContext db = new StockDBContext())
            {
                var groups = db.StockQuotes.Include("Stock").Where(s => /*DbFunctions.DiffDays(DateTime.Today, s.DateBought) < 5 && */ s.PortfolioCode == portfolioCode).ToList().GroupBy(s => s.StockID);
                StockQuote sq = null;
                foreach (var group in groups)
                {
                    sq = group.OrderByDescending(s => s.DateBought).First();
                    if (sq.Owned)
                    {
                        result.Add(sq);
                    }
                }
            }

            return result;
        }

        public static bool StockQuoteExists(StockQuote stockQuote)
        {
            bool exists = false;

            using (StockDBContext db = new StockDBContext())
            {
                var latest = db.StockQuotes.Where(s => s.Stock.Ticker.Equals(stockQuote.Stock.Ticker)).OrderByDescending(s => s.DateBought).FirstOrDefault();
                if (latest != null && latest.Owned == stockQuote.Owned)
                {
                    exists = true;
                } 
            }

            return exists;
        }

        public static List<StockQuote> ListFinishedTradesStockQuotes()
        {
            List<StockQuote> result = null;
            using (StockDBContext db = new StockDBContext())
            {
                result = db.StockQuotes.Include("Stock").Where(s => s.Owned == false).ToList();
            }

            return result;
        }

        public static Portfolio GetPortfolio(int PortfolioCode)
        {
            using (StockDBContext db = new StockDBContext())
            {
                return db.Portfolio.Where(p => p.PortfolioCode == PortfolioCode).OrderByDescending(p => p.Date).First();
            }
        }

        public static decimal GetPortfolioFunds(int PortfolioCode)
        {
            using (StockDBContext db = new StockDBContext())
            {
                return db.Portfolio.Where(p => p.PortfolioCode == PortfolioCode).OrderByDescending(p => p.Date).First().Funds;
            }
        }

        public static void UpdatePortfolioFunds(int PortfolioCode, decimal NewFunds)
        {
            using (StockDBContext db = new StockDBContext())
            {
                Portfolio portfolio = db.Portfolio.Where(p => p.PortfolioCode == PortfolioCode).OrderByDescending(p => p.Date).First();
                if (portfolio.Date.Day != DateTime.Today.Day)
                {
                    Portfolio newPortfolio = portfolio;
                    newPortfolio.Date = DateTime.Now;
                    newPortfolio.Funds = NewFunds;
                    db.Portfolio.Add(newPortfolio);
                }
                else
                {
                    portfolio.Funds = NewFunds;
                }
                
                db.SaveChanges();
            }
        }

        internal static void UpdatePortfolioInvestedValue(int PortfolioCode, decimal investedValue)
        {
            using (StockDBContext db = new StockDBContext())
            {
                Portfolio portfolio = db.Portfolio.Where(p => p.PortfolioCode == PortfolioCode).OrderByDescending(p => p.Date).First();
                if (portfolio.Date.Day != DateTime.Today.Day)
                {
                    Portfolio newPortfolio = portfolio;
                    newPortfolio.Date = DateTime.Now;
                    newPortfolio.InvestedValue = investedValue;
                    db.Portfolio.Add(newPortfolio);
                }
                else
                {
                    portfolio.InvestedValue = investedValue;
                }
                
                db.SaveChanges();
            }
        }
    }
}
