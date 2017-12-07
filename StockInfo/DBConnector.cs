using MySql.Data.MySqlClient;
using StockInfo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo
{
    public class StockDBContext : DbContext
    {
        public StockDBContext() : base("DBContext")
        {

        }
        //Override default table names
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static StockDBContext Create()
        {
            return new StockDBContext();
        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
