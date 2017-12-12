using StockInfo.DB;
using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StockInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            //MailHelper.SendEmail(new List<string> { "joakimhellerstrom@hotmail.com" }, QuoteData, Int32.Parse(ConfigurationManager.AppSettings["portfolioCode"]));
            //StockScanner.ScanHistoryOfStock(new DateTime(2017, 9, 1), DateTime.Now);
            //List<DailyTimeSeries> result = StockScanner.ScanStockMarket(StrategyType.All);
            //MailHelper.CreateBodyToFile(result);
            //DailyTimeSeries data = StockTimeSeriesFetcher.GetDailyAdjustedTimeSeries("ABB.ST");
            RunRicoschettAnalysis();
        }

        public static void RunRicoschettAnalysis()
        {
            StringBuilder sb = new StringBuilder();

            List<string> tickers; 
            using (StockDBContext db = new StockDBContext())
            {
                tickers = db.Stocks.Select(s => s.Ticker).ToList();
            }

            DateTime fromDate = DateTime.Today.AddDays(-14);

            List<DailyTimeSeries> timeSeries = TimeSeriesFetcher.ListDailyAdjustedTimeSeries(tickers, OutputSizeType.Compact);
            List<DailyTimeSeries> ricoStocks = null;
            List<DailyTimeSeries> stocksToSell = null;
            while (!fromDate.Equals(DateTime.Today))
            {
                if (ricoStocks != null)
                {
                    stocksToSell = Calculator.CalculateOwnedRicoschettQuotes(ricoStocks, fromDate);
                }

                //Must merge ricostocks with new ricostocks!!!!!
                ricoStocks = Calculator.CalculateRicoschettQuotes(timeSeries, fromDate);

                sb.AppendLine("Stocks bought " + fromDate.ToString());
                foreach (var rs in ricoStocks)
                {
                    sb.AppendLine(rs.MetaData.Symbol + " \tBought price: " + rs.TradeInfo.BoughtPrice.ToString("N2") 
                        + " \tOpen: " + rs.TradeInfo.TimeSeriesData.Open.ToString("N2") 
                        + " \tClose: " + rs.TradeInfo.TimeSeriesData.Close.ToString("N2")
                        + " \tPercent: " + ((rs.TradeInfo.TimeSeriesData.Close - rs.TradeInfo.TimeSeriesData.Open) / rs.TradeInfo.TimeSeriesData.Open).ToString("N2"));
                }

                if (stocksToSell != null)
                {
                    sb.AppendLine("Stocks sold " + fromDate.ToString());
                    foreach (var rs in stocksToSell)
                    {
                        TimeSeriesData tsd = rs.TimeSeries.Where(k => k.Key.Equals(fromDate)).First().Value;
                        sb.AppendLine(rs.MetaData.Symbol + " \tBought price: " + rs.TradeInfo.BoughtPrice.ToString("N2")
                            + " \tSold price: " + tsd.Close.ToString("N2")
                            + " \tProfit/Loss: " + (tsd.Close - rs.TradeInfo.BoughtPrice).ToString("N2")
                            + " \tPercentage: " + ((tsd.Close - rs.TradeInfo.BoughtPrice) / rs.TradeInfo.BoughtPrice).ToString("N2")
                            );
                    }
                }

                fromDate = fromDate.AddDays(1);
                while (fromDate.DayOfWeek.ToString().Equals("Saturday") || fromDate.DayOfWeek.ToString().Equals("Sunday"))
                {
                    fromDate = fromDate.AddDays(1);
                }
            }

            string path = @".\RicoschettAnalysis.txt";

            try
            {

                // Delete the file if it exists.
                if (File.Exists(path))
                {
                    // Note that no lock is put on the
                    // file and the possibility exists
                    // that another process could do
                    // something with it between
                    // the calls to Exists and Delete.
                    File.Delete(path);
                }

                // Create the file.
                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
