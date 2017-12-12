using StockInfo.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo.Entities
{
    public class TimeSeriesDataInfo
    {
        public int ID { get; set; }
        public TimeSeriesData TimeSeriesData { get; set; }
        public int StockID { get; set; }
        public Stock Stock { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime TimeStampFetched { get; set; }

        public TimeSeriesDataInfo() { }
        public TimeSeriesDataInfo(DailyTimeSeries dts)
        {
            TimeSeriesData = dts.TimeSeries.First().Value;
            using (StockDBContext db = new StockDBContext())
            {
                Stock = db.Stocks.Where(s => s.Ticker == dts.MetaData.Symbol).First();
                StockID = Stock.ID;
            }
            TimeStamp = dts.TimeSeries.First().Key;
            TimeStampFetched = DateTime.Now;
        }
    }
}
