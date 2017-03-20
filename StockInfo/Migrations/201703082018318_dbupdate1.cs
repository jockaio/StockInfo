namespace StockInfo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockQuotes", "StrategyType", c => c.Int(nullable: false));
            CreateIndex("dbo.StockQuotes", "StockID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StockQuotes", new[] { "StockID" });
            DropColumn("dbo.StockQuotes", "StrategyType");
            CreateIndex("dbo.StockQuotes", "StockId");
        }
    }
}
