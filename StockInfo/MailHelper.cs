using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace StockInfo
{
    public static class MailHelper
    {
        public static bool SendEmail(List<string> recievers)
        {
            string host = ConfigurationManager.AppSettings["host"];
            string sender = ConfigurationManager.AppSettings["usrName"];
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(sender);
            foreach (var reciever in recievers)
            {
                msg.To.Add(reciever);
            }
            msg.Subject = "Today's StockInfo";
            msg.Body = CreateBody();

            Console.Write(msg.Body);

            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["usrName"], ConfigurationManager.AppSettings["pass"]);
            client.EnableSsl = true;
            client.Timeout = 10000;

            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                msg.Dispose();
            }
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
        }

        public static string CreateBody()
        {
            List<string> Tickers = new List<string>();
            using (StockDBContext db = new StockDBContext())
            {
                Tickers = db.Stocks.Select(s => s.Ticker).ToList();
            }
            Results QuoteData = QuoteDataFetcher.GetQuoteData(Tickers);

            QuoteData.quote = QuoteData.quote.OrderByDescending(q => decimal.Parse(q.ChangeinPercent.Remove(q.ChangeinPercent.Count()-1), CultureInfo.InvariantCulture)).ToList();

            string Body = "<h1>Dagens OMXS30</h1>";
            Body += "<table>";
            Body += "<tr>";
            Body += "<th>Symbol</th>";
            Body += "<th>Change (%)</th>";
            Body += "<th>Change (SEK)</th>";
            Body += "<th>Last Price (SEK)</th>";
            Body += "</tr>";

            foreach (var quote in QuoteData.quote)
            {
                Body += "<tr>";
                Body += "<td>" + quote.Symbol + "</td>";
                Body += "<td>" + quote.ChangeinPercent + "%</td>";
                Body += "<td>" + quote.Change + " SEK </td>";
                Body += "<td>" + quote.LastTradePriceOnly + " SEK</td>";
                Body += "</tr>";
                //Body += quote.Symbol + "\t " + quote.ChangeinPercent + "%\t " + quote.Change + "SEK" + "\t " + quote.LastTradePriceOnly + "SEK\n";
            }

            Body += "</table>";

            List<Quote> RicoschettQuotes = Calculator.CalculateRicoschettQuotes(QuoteData.quote);
            Calculator.CalculateOwnedRicoschettQuotes(QuoteData.quote);

            Body += "<h1>Ricoschett-strategy<h1>";
            Body += "<table>";
            Body += "<tr>";
            Body += "<th>Symbol</th>";
            Body += "<th>Change (%)</th>";
            Body += "<th>Change (SEK)</th>";
            Body += "<th>Last Price (SEK)</th>";
            Body += "</tr>";
            foreach (var quote in RicoschettQuotes)
            {
                Body += "<tr>";
                Body += "<td>" + quote.Symbol + "</td>";
                Body += "<td>" + quote.ChangeinPercent + "%</td>";
                Body += "<td>" + quote.Change + " SEK </td>";
                Body += "<td>" + quote.LastTradePriceOnly + " SEK</td>";
                Body += "</tr>";
                //Body += quote.Symbol + "\t " + quote.ChangeinPercent + "\t " + quote.Change + "SEK" + "\t " + quote.LastTradePriceOnly + "SEK " + quote.LastTradeTime + "\n";
            }
            Body += "</table>";


            Quote qt;
            Body += "<h1>Ricoschett-portfolio<h1>";
            List<StockQuote> OwnedQuotes = Storage.ListOwnedStockQuotes();
            Body += "<table>";
            Body += "<tr>";
            Body += "<th>TradeDate: </th>";
            Body += "<th>Symbol</th>";
            Body += "<th>BoughtPrice</th>";
            Body += "<th>LastPrice</th>";
            Body += "<th>Last trade time</th>";
            Body += "<th>Change (%)</th>";
            Body += "</tr>";
            foreach (var quote in OwnedQuotes)
            {
                qt = QuoteData.quote.Where(qte => qte.Symbol.ToLower().Equals(quote.Stock.Ticker.ToLower())).First();
                Body += "<tr>";
                Body += "<td>" + quote.DateBought.ToString("d") + "</td>";
                Body += "<td>" + quote.Stock.Name + "</td>";
                Body += "<td>" + quote.Price + " SEK </td>";
                Body += "<td>" + qt.LastTradePriceOnly + " SEK </td>";
                Body += "<td>" + qt.LastTradeTime + " SEK</td>";
                Body += "<td>" + (((decimal.Parse(qt.LastTradePriceOnly, CultureInfo.InvariantCulture) / quote.Price) - 1) * 100).ToString("N2") + " SEK</td>";
                Body += "</tr>";
                //Body += "TradeDate: " + quote.DateBought.ToString("d") + " " + quote.Stock.Name + "\t BoughtPrice: " + quote.Price + "\t LastPrice:" + qt.LastTradePriceOnly + "SEK " + qt.LastTradeTime + "\tChange (%): " + (((decimal.Parse(qt.LastTradePriceOnly, CultureInfo.InvariantCulture) / quote.Price) - 1) * 100).ToString("N2") + "%\n";
            }
            Body += "</table>";

            Body += "<h1>Ricoschett - Latest deals<h1>";
            List<StockQuote> FinisehdTradesQuotes = Storage.ListFinishedTradesStockQuotes();

            Body += "<table>";
            Body += "<tr>";
            Body += "<th>TradeDate: </th>";
            Body += "<th>Symbol</th>";
            Body += "<th>BoughtPrice</th>";
            Body += "<th>Change (%)</th>";
            Body += "<th>Change (SEK)</th>";
            Body += "</tr>";
            foreach (var quote in FinisehdTradesQuotes)
            {
                Body += "<tr>";
                Body += "<td>" + quote.DateBought.ToString("d") + "</td>";
                Body += "<td>" + quote.Stock.Name + "%</td>";
                Body += "<td>" + quote.Price + " SEK</td>";
                Body += "<td>" + (((quote.Price / (quote.Price - quote.Change.Value)) - 1) * 100).ToString("N2") + "% </td>";
                Body += "<td>" + quote.Change + " SEK</td>";
                Body += "</tr>";
                //Body += "TradeDate: " + quote.DateBought.ToString("d") + " " + quote.Stock.Name + "\t SoldPrice: " + quote.Price + "\tChange: " + (((quote.Price / (quote.Price - quote.Change.Value)) - 1) * 100).ToString("N2") + "%\tChange (SEK): " + quote.Change + "\n";
            }
            Body += "</table>";

            return Body;
        }

        public static void CreateBodyToFile()
        {
            string Body = CreateBody();

            string path = @".\body.txt";

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
