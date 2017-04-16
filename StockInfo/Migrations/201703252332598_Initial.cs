namespace StockInfo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PortfolioCode = c.Int(nullable: false),
                        Funds = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvestedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StockQuotes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false, precision: 0),
                        StockID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Owned = c.Boolean(nullable: false),
                        DateBought = c.DateTime(nullable: false, precision: 0),
                        StrategyType = c.Int(nullable: false),
                        Change = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        PortfolioCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Stocks", t => t.StockID, cascadeDelete: false)
                .Index(t => t.StockID);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Ticker = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockQuotes", "StockID", "dbo.Stocks");
            DropIndex("dbo.StockQuotes", new[] { "StockID" });
            DropTable("dbo.Stocks");
            DropTable("dbo.StockQuotes");
            DropTable("dbo.Portfolios");
        }
    }
}
