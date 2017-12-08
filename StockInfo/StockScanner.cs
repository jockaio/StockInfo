using StockInfo.DB;
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
        public static List<DailyTimeSeries> ScanStockMarket(StrategyType StrategyType)
        {
            List<DailyTimeSeries> result = null;
            switch (StrategyType)
            {
                case StrategyType.Ricoschett:
                    result = ScanForRicoschett();
                    break;
                case StrategyType.All:
                    result = GetAllStocks();
                    break;
                default:
                    break;
            }

            return result;
        }

        private static List<DailyTimeSeries> GetAllStocks()
        {
            List<string> tickers = new List<string>();
            using (StockDBContext db = new StockDBContext())
            {
                tickers = db.Stocks.Select(s => s.Ticker).ToList();
            }

            List<DailyTimeSeries> dts = TimeSeriesFetcher.ListDailyAdjustedTimeSeries(tickers, OutputSizeType.Compact);

            return dts;
        }

        //public static List<Results> ScanHistoryOfStock(DateTime startDate, DateTime endDate)
        //{
        //    List<string> Tickers = new List<string>();
        //    using (StockDBContext db = new StockDBContext())
        //    {
        //        Tickers = db.Stocks.Select(s => s.Ticker).ToList();
        //    }
        //    List<Results> result = QuoteDataFetcher.GetHistoricQuoteData(Tickers, startDate, endDate);

        //    return result;
        //}

        private static List<DailyTimeSeries> ScanForRicoschett()
        {
            List<string> tickers = new List<string>();
            using (StockDBContext db = new StockDBContext())
            {
                tickers = db.Stocks.Select(s => s.Ticker).ToList();
            }

            List<DailyTimeSeries> dts = TimeSeriesFetcher.ListDailyAdjustedTimeSeries(tickers, OutputSizeType.Compact);

            List<DailyTimeSeries> todaysRicoschetts = Calculator.CalculateRicoschettQuotes(dts);

            return todaysRicoschetts;
        }
    }
}
