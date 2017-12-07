using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            //Results QuoteData = StockScanner.ScanStockMarket(StrategyType.Ricoschett);
            //MailHelper.SendEmail(new List<string> { "joakimhellerstrom@hotmail.com" }, QuoteData, Int32.Parse(ConfigurationManager.AppSettings["portfolioCode"]));
            //StockScanner.ScanHistoryOfStock(new DateTime(2017, 9, 1), DateTime.Now);
            //MailHelper.CreateBodyToFile(QuoteData, Int32.Parse(ConfigurationManager.AppSettings["portfolioCode"]));
            //DailyTimeSeries data = StockTimeSeriesFetcher.GetDailyAdjustedTimeSeries("ABB.ST");
        }
    }
}
