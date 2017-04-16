using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StockInfo
{
    public static class QuoteDataFetcher
    {
        public static Results GetQuoteData(List<string> tickers)
        {
            string urlTickers = tickers.Aggregate((i, j) => i + "+" + j);

            string url =
                "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%3D%27"
                + urlTickers
                + "%27&format=json&diagnostics=false&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(url);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RootObject rootObject = new RootObject();
                rootObject = serializer.Deserialize<RootObject>(json);

                return rootObject.query.results;
            }
        }

        public static List<Results> GetHistoricQuoteData(List<string> tickers, DateTime fromDate, DateTime endDate)
        {
            List<Results> results = new List<Results>();
            string url = "";
            using (WebClient wc = new WebClient())
            {
                foreach (var ticker in tickers)
                {
                    url =
                    "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.historicaldata%20where%20symbol%20%3D%20%22"
                    + ticker
                    + "%22%20and%20startDate%20%3D%20%22"
                    + fromDate.ToString("yyyy-MM-dd")
                    + "%22%20and%20endDate%20%3D%20%22"
                    + endDate.ToString("yyyy-MM-dd")
                    + "%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

                    string json = wc.DownloadString(url);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    RootObject rootObject = new RootObject();
                    rootObject = serializer.Deserialize<RootObject>(json);
                    results.Add(rootObject.query.results);
                }

                return results;
            }
        }

        public static Results GetQuoteDataMockup()
        {
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(@"C:\Users\joaki\Documents\Visual Studio 2015\Projects\StockInfo\testfile.json");

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RootObject rootObject = new RootObject();
                rootObject = serializer.Deserialize<RootObject>(json);

                return rootObject.query.results;
            }
        }
    }
}
