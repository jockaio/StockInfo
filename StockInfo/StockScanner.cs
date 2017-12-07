using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo
{
    public static class StockScanner
    {
        public static Results ScanStockMarket(StrategyType StrategyType)
        {
            Results result = null; 
            switch (StrategyType)
            {
                case StrategyType.Ricoschett:
                    result = ScanForRicoschett();
                    break;
                default:
                    break;
            }

            return result;
        }

        public static List<Results> ScanHistoryOfStock(DateTime startDate, DateTime endDate)
        {
            List<string> Tickers = new List<string>();
            using (StockDBContext db = new StockDBContext())
            {
                Tickers = db.Stocks.Select(s => s.Ticker).ToList();
            }
            List<Results> result = QuoteDataFetcher.GetHistoricQuoteData(Tickers, startDate, endDate);

            return result;
        }

        private static Results ScanForRicoschett()
        {
            List<string> Tickers = new List<string>();
            using (StockDBContext db = new StockDBContext())
            {
                Tickers = db.Stocks.Select(s => s.Ticker).ToList();
            }
            Results QuoteData = QuoteDataFetcher.GetQuoteData(Tickers);

            QuoteData.quote = QuoteData.quote.OrderByDescending(q => decimal.Parse(q.ChangeinPercent.Remove(q.ChangeinPercent.Count() - 1), CultureInfo.InvariantCulture)).ToList();

            //Check owned stocks against todays data.
            Calculator.CalculateOwnedRicoschettQuotes(QuoteData.quote, Int32.Parse(ConfigurationManager.AppSettings["portfolioCode"]));

            List<Quote> TodaysRicoschetts = Calculator.CalculateRicoschettQuotes(QuoteData.quote);
            Calculator.AllocatePortfolioFunds(TodaysRicoschetts, Int32.Parse(ConfigurationManager.AppSettings["portfolioCode"]));

            return QuoteData;
        }
    }
}
