// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using StockInfo.Entities;
//
//    var data = DailyTimeSeries.FromJson(jsonString);
//
namespace StockInfo.Entities
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class DailyTimeSeries
    {
        [JsonProperty("Meta Data")]
        public DailyMetaData MetaData { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<DateTime, TimeSeriesData> TimeSeries { get; set; }

        public TradeInfo TradeInfo { get; set; }
    }

    public class TradeInfo
    {
        public DateTime BoughtDate { get; set; }
        public decimal BoughtPrice { get; set; }
        public TimeSeriesData TimeSeriesData { get; set; }
    }

    public partial class TimeSeriesData
    {
        [JsonProperty("1. open")]
        public decimal Open { get; set; }

        [JsonProperty("2. high")]
        public decimal High { get; set; }

        [JsonProperty("3. low")]
        public decimal Low { get; set; }

        [JsonProperty("4. close")]
        public decimal Close { get; set; }

        [JsonProperty("5. adjusted close")]
        public decimal AdjustedClose { get; set; }

        [JsonProperty("6. volume")]
        public int Volume { get; set; }

        [JsonProperty("7. dividend amount")]
        public decimal DividendAmount { get; set; }

        [JsonProperty("8. split coefficient")]
        public decimal SplitCoefficient { get; set; }
    }

    public partial class DailyMetaData
    {
        [JsonProperty("1. Information")]
        public string Information { get; set; }

        [JsonProperty("2. Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonProperty("4. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty("5. Time Zone")]
        public string TimeZone { get; set; }
    }

    public partial class DailyTimeSeries
    {
        public static DailyTimeSeries FromJson(string json) => JsonConvert.DeserializeObject<DailyTimeSeries>(json, Converter.Settings);
    }
}
