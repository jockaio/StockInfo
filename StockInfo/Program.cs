using System;
using System.Collections.Generic;
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

            //MailHelper.SendEmail(new List<string> { "joakimhellerstrom@hotmail.com" });
            MailHelper.CreateBodyToFile();

        }
    }
}
