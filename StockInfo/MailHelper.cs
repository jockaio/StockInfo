using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using StockInfo.Entities;

namespace StockInfo
{
    public static class MailHelper
    {
        //public static bool SendEmail(List<string> recievers, Results QuoteData, int portfolioCode)
        //{
        //    string host = ConfigurationManager.AppSettings["host"];
        //    string sender = ConfigurationManager.AppSettings["usrName"];
        //    MailMessage msg = new MailMessage();
        //    msg.IsBodyHtml = true;
        //    msg.From = new MailAddress(sender);
        //    foreach (var reciever in recievers)
        //    {
        //        msg.To.Add(reciever);
        //    }
        //    msg.Subject = "Today's StockInfo";
        //    msg.Body = CreateBody(QuoteData, portfolioCode);

        //    Console.Write(msg.Body);

        //    SmtpClient client = new SmtpClient();
        //    client.Host = host;
        //    client.Port = 587;
        //    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["usrName"], ConfigurationManager.AppSettings["pass"]);
        //    client.EnableSsl = true;
        //    client.Timeout = 10000;

        //    try
        //    {
        //        client.Send(msg);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        msg.Dispose();
        //    }
        //string host = "smtp.bredband.net";
        //string sender = "joakim.hellerstrom@bredband.net";

        //MailMessage msg = new MailMessage();
        //msg.From = new MailAddress(sender);
        //foreach (var reciever in recievers)
        //{
        //    msg.To.Add(reciever);
        //}
        //msg.Subject = "Today's StockInfo";
        //msg.Body = CreateBody();

        //Console.Write(msg.Body);

        //SmtpClient client = new SmtpClient();
        //client.Host = host;
        //client.Port = 25;
        //client.Timeout = 10000;

        //try
        //{
        //    client.Send(msg);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Error: " + ex.Message);
        //    return false;
        //}
        //finally
        //{
        //    msg.Dispose();
        //}
        //}

        public static string CreateBody(List<DailyTimeSeries> dailyTimeSeries)
        {
            dailyTimeSeries = dailyTimeSeries.OrderBy(d => (d.TimeSeries.First().Value.Close - d.TimeSeries.First().Value.Open) / d.TimeSeries.First().Value.Open).ToList();

            string Body = "<h1>Dagens OMXS30</h1>";
            Body += "<table>";
            Body += "<tr>";
            Body += "<td>Symbol</td>";
            Body += "<td>Change (%)</td>";
            Body += "<td>Change (SEK)</td>";
            Body += "<td>High</td>";
            Body += "<td>Low</td>";
            Body += "<td>Last Price (SEK)</td>";
            Body += "</tr>";

            foreach (var dt in dailyTimeSeries)
            {
                Body += "<tr>";
                Body += "<td>" + dt.MetaData.Symbol + "</td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Close - dt.TimeSeries.First().Value.Open) / dt.TimeSeries.First().Value.Open + "%</td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Close - dt.TimeSeries.First().Value.Open) + " SEK </td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.High) + " SEK </td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Low) + " SEK </td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Close) + " SEK</td>";
                Body += "</tr>";
            }

            Body += "</table>";

            List<DailyTimeSeries> RicoschettQuotes = Calculator.CalculateRicoschettQuotes(dailyTimeSeries);

            Body += "<h1>Ricoschett-strategy<h1>";
            Body += "<table>";
            Body += "<tr>";
            Body += "<td>Symbol</td>";
            Body += "<td>Change (%)</td>";
            Body += "<td>Change (SEK)</td>";
            Body += "<td>High</td>";
            Body += "<td>Low</td>";
            Body += "<td>Last Price (SEK)</td>";
            Body += "</tr>";
            foreach (var dt in RicoschettQuotes)
            {
                Body += "<tr>";
                Body += "<td>" + dt.MetaData.Symbol + "</td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Close - dt.TimeSeries.First().Value.Open) / dt.TimeSeries.First().Value.Open + "%</td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Close - dt.TimeSeries.First().Value.Open) + " SEK </td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.High) + " SEK </td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Low) + " SEK </td>";
                Body += "<td>" + (dt.TimeSeries.First().Value.Close) + " SEK</td>";
                Body += "</tr>";
                //Body += quote.Symbol + "\t " + quote.ChangeinPercent + "\t " + quote.Change + "SEK" + "\t " + quote.LastTradePriceOnly + "SEK " + quote.LastTradeTime + "\n";
            }
            Body += "</table>";

            #region Old stuff
            //Quote qt;
            //Body += "<h1>Ricoschett-portfolio<h1>";
            //List<StockQuote> OwnedQuotes = Storage.ListOwnedStockQuotes(portfolioCode);
            //Body += "<table>";
            //Body += "<tr>";
            //Body += "<td>TradeDate: </td>";
            //Body += "<td>Symbol</td>";
            //Body += "<td>BoughtPrice</td>";
            //Body += "<td>LastPrice</td>";
            //Body += "<td>Last trade time</td>";
            //Body += "<td>Change (%)</td>";
            //Body += "<td>Value</td>";
            //Body += "</tr>";
            //foreach (var quote in OwnedQuotes)
            //{
            //    qt = QuoteData.quote.Where(qte => qte.Symbol.ToLower().Equals(quote.Stock.Ticker.ToLower())).First();
            //    Body += "<tr>";
            //    Body += "<td>" + quote.DateBought.ToString("d") + "</td>";
            //    Body += "<td>" + quote.Stock.Name + "</td>";
            //    Body += "<td>" + quote.Price + " SEK </td>";
            //    Body += "<td>" + qt.LastTradePriceOnly + " SEK </td>";
            //    Body += "<td>" + qt.LastTradeTime + " SEK</td>";
            //    Body += "<td>" + (((decimal.Parse(qt.LastTradePriceOnly, CultureInfo.InvariantCulture) / quote.Price) - 1) * 100).ToString("N2") + "%</td>";
            //    Body += "<td>" + quote.LastPrice * quote.Quantity + " SEK</td>";
            //    Body += "</tr>";
            //    //Body += "TradeDate: " + quote.DateBought.ToString("d") + " " + quote.Stock.Name + "\t BoughtPrice: " + quote.Price + "\t LastPrice:" + qt.LastTradePriceOnly + "SEK " + qt.LastTradeTime + "\tChange (%): " + (((decimal.Parse(qt.LastTradePriceOnly, CultureInfo.InvariantCulture) / quote.Price) - 1) * 100).ToString("N2") + "%\n";
            //}
            //Body += "</table>";

            //Portfolio Portfolio = Storage.GetPortfolio(portfolioCode);
            //Body += "<h1>Total Portfolio Data<h1>";
            //Body += "<table>";
            //Body += "<tr>";
            //Body += "<td>Date</td>";
            //Body += "<td>Funds</td>";
            //Body += "<td>Invested</td>";
            //Body += "<td>Total value</td>";
            //Body += "</tr>";
            //Body += "<tr>";
            //Body += "<td>" + Portfolio.Date.ToString("d") + "</td>";
            //Body += "<td>" + Portfolio.Funds.ToString("N2") + "</td>";
            //Body += "<td>" + Portfolio.InvestedValue.ToString("N2") + "</td>";
            //Body += "<td>" + (Portfolio.InvestedValue + Portfolio.Funds).ToString("N2") + "</td>";
            //Body += "</tr>";
            //Body += "</table>";

            //Body += "<h1>Ricoschett - Latest deals<h1>";
            //List<StockQuote> FinisehdTradesQuotes = Storage.ListFinishedTradesStockQuotes();

            //Body += "<table>";
            //Body += "<tr>";
            //Body += "<td>TradeDate: </td>";
            //Body += "<td>Symbol</td>";
            //Body += "<td>BoughtPrice</td>";
            //Body += "<td>Change (%)</td>";
            //Body += "<td>Change (SEK)</td>";
            //Body += "</tr>";
            //foreach (var quote in FinisehdTradesQuotes)
            //{
            //    Body += "<tr>";
            //    Body += "<td>" + quote.DateBought.ToString("d") + "</td>";
            //    Body += "<td>" + quote.Stock.Name + "%</td>";
            //    Body += "<td>" + quote.Price + " SEK</td>";
            //    Body += "<td>" + (((quote.Price / (quote.Price - quote.Change.Value)) - 1) * 100).ToString("N2") + "% </td>";
            //    Body += "<td>" + quote.Change + " SEK</td>";
            //    Body += "</tr>";
            //    //Body += "TradeDate: " + quote.DateBought.ToString("d") + " " + quote.Stock.Name + "\t SoldPrice: " + quote.Price + "\tChange: " + (((quote.Price / (quote.Price - quote.Change.Value)) - 1) * 100).ToString("N2") + "%\tChange (SEK): " + quote.Change + "\n";
            //}
            //Body += "</table>";
            #endregion
            return Body;
        }

        public static void CreateBodyToFile(List<DailyTimeSeries> dailyTimeSeries)
        {
            string Body = CreateBody(dailyTimeSeries);

            string path = @".\body.html";

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
                    Byte[] info = new UTF8Encoding(true).GetBytes(Body);
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
