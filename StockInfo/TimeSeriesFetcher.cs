﻿using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo
{
    public static class TimeSeriesFetcher
    {
        public static DailyTimeSeries GetDailyAdjustedTimeSeries(string ticker)
        {
            string url =
                "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=" + ticker +
                "&outputsize=compact&apikey=TANIY6VHG6KO50WE";

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(url);
                DailyTimeSeries data = DailyTimeSeries.FromJson(json);
                return data;
            }
        }

        public static IntradayTimeSeries GetIntradayTimeSeries(string ticker, int interval, OutputSizeType outputSizeType)
        {
            string url =
                "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + ticker +
                "&interval=" + interval + "min&outputsize=" + outputSizeType.ToString() + "& apikey=TANIY6VHG6KO50WE";

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(url);
                IntradayTimeSeries data = IntradayTimeSeries.FromJson(json);
                return data;
            }
        }

        public static List<IntradayTimeSeries> ListIntradayTimeSeries(List<string> tickers, int interval, OutputSizeType outputSizeType)
        {
            List<IntradayTimeSeries> result = new List<IntradayTimeSeries>();
            foreach (var ticker in tickers)
            {
                IntradayTimeSeries its = GetIntradayTimeSeries(ticker, interval, outputSizeType);
                if (its.MetaData != null && its.TimeSeries != null && its.TimeSeries.Count > 0)
                {
                    result.Add(its);
                }
            }

            return result;
        }
    }
}
