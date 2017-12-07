// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using StockInfo.Entities;
//
//    var data = IntradayTimeSeries.FromJson(jsonString);
//
namespace StockInfo.Entities
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using System.Text.RegularExpressions;

    public partial class IntradayTimeSeries
    {
        [JsonProperty("Meta Data")]
        public IntradayMetaData MetaData { get; set; }

        [JsonProperty("Time Series")]
        public Dictionary<DateTime, IntradayTimeSeriesData> TimeSeries { get; set; }
    }

    public partial class IntradayTimeSeriesData
    {
        [JsonProperty("1. open")]
        public decimal Open { get; set; }

        [JsonProperty("2. high")]
        public decimal High { get; set; }

        [JsonProperty("3. low")]
        public decimal Low { get; set; }

        [JsonProperty("4. close")]
        public decimal Close { get; set; }

        [JsonProperty("5. volume")]
        public int Volume { get; set; }
    }

    public partial class IntradayMetaData
    {
        [JsonProperty("1. Information")]
        public string Information { get; set; }

        [JsonProperty("2. Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonProperty("4. Interval")]
        public string Interval { get; set; }

        [JsonProperty("5. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty("6. Time Zone")]
        public string TimeZone { get; set; }
    }

    public partial class IntradayTimeSeries
    {
        public static IntradayTimeSeries FromJson(string json)
        {
            string pattern = @"Time Series \([0-9]*min\)";
            string replacement = "Time Series";
            Regex rgx = new Regex(pattern);
            json = rgx.Replace(json, replacement);
            return JsonConvert.DeserializeObject<IntradayTimeSeries>(json, Converter.Settings);
        }
    }
}
