using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo
{
    public class Quote
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
        public string Adj_Close { get; set; }
        public string Ask { get; set; }
        public string AverageDailyVolume { get; set; }
        public string Bid { get; set; }
        public object AskRealtime { get; set; }
        public object BidRealtime { get; set; }
        public string BookValue { get; set; }
        public string Change_PercentChange { get; set; }
        public string Change { get; set; }
        public object Commission { get; set; }
        public string Currency { get; set; }
        public object ChangeRealtime { get; set; }
        public object AfterHoursChangeRealtime { get; set; }
        public string DividendShare { get; set; }
        public string LastTradeDate { get; set; }
        public object TradeDate { get; set; }
        public string EarningsShare { get; set; }
        public object ErrorIndicationreturnedforsymbolchangedinvalid { get; set; }
        public string EPSEstimateCurrentYear { get; set; }
        public object EPSEstimateNextYear { get; set; }
        public string EPSEstimateNextQuarter { get; set; }
        public string DaysLow { get; set; }
        public string DaysHigh { get; set; }
        public string YearLow { get; set; }
        public string YearHigh { get; set; }
        public object HoldingsGainPercent { get; set; }
        public object AnnualizedGain { get; set; }
        public object HoldingsGain { get; set; }
        public object HoldingsGainPercentRealtime { get; set; }
        public object HoldingsGainRealtime { get; set; }
        public object MoreInfo { get; set; }
        public object OrderBookRealtime { get; set; }
        public string MarketCapitalization { get; set; }
        public object MarketCapRealtime { get; set; }
        public string EBITDA { get; set; }
        public string ChangeFromYearLow { get; set; }
        public string PercentChangeFromYearLow { get; set; }
        public object LastTradeRealtimeWithTime { get; set; }
        public object ChangePercentRealtime { get; set; }
        public string ChangeFromYearHigh { get; set; }
        public string PercebtChangeFromYearHigh { get; set; }
        public string LastTradeWithTime { get; set; }
        public string LastTradePriceOnly { get; set; }
        public object HighLimit { get; set; }
        public object LowLimit { get; set; }
        public string DaysRange { get; set; }
        public object DaysRangeRealtime { get; set; }
        public string FiftydayMovingAverage { get; set; }
        public string TwoHundreddayMovingAverage { get; set; }
        public string ChangeFromTwoHundreddayMovingAverage { get; set; }
        public string PercentChangeFromTwoHundreddayMovingAverage { get; set; }
        public string ChangeFromFiftydayMovingAverage { get; set; }
        public string PercentChangeFromFiftydayMovingAverage { get; set; }
        public string Name { get; set; }
        public object Notes { get; set; }
        public string PreviousClose { get; set; }
        public object PricePaid { get; set; }
        public string ChangeinPercent { get; set; }
        public string PriceSales { get; set; }
        public string PriceBook { get; set; }
        public string ExDividendDate { get; set; }
        public string PERatio { get; set; }
        public object DividendPayDate { get; set; }
        public object PERatioRealtime { get; set; }
        public string PEGRatio { get; set; }
        public object PriceEPSEstimateCurrentYear { get; set; }
        public object PriceEPSEstimateNextYear { get; set; }
        public object SharesOwned { get; set; }
        public string ShortRatio { get; set; }
        public string LastTradeTime { get; set; }
        public object TickerTrend { get; set; }
        public object OneyrTargetPrice { get; set; }
        public object HoldingsValue { get; set; }
        public object HoldingsValueRealtime { get; set; }
        public string YearRange { get; set; }
        public object DaysValueChange { get; set; }
        public object DaysValueChangeRealtime { get; set; }
        public string StockExchange { get; set; }
        public string DividendYield { get; set; }
        public string PercentChange { get; set; }
    }

    public class Results
    {
        public List<Quote> quote { get; set; }
    }

    public class Query
    {
        public int count { get; set; }
        public string created { get; set; }
        public string lang { get; set; }
        public Results results { get; set; }
    }

    public class RootObject
    {
        public Query query { get; set; }
    }

}
