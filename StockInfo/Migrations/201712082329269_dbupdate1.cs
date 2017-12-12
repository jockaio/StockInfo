namespace StockInfo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeSeriesDataInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeSeriesData_Open = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeSeriesData_High = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeSeriesData_Low = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeSeriesData_Close = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeSeriesData_AdjustedClose = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeSeriesData_Volume = c.Int(nullable: false),
                        TimeSeriesData_DividendAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeSeriesData_SplitCoefficient = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Stocks", t => t.StockID, cascadeDelete: true)
                .Index(t => t.StockID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSeriesDataInfoes", "StockID", "dbo.Stocks");
            DropIndex("dbo.TimeSeriesDataInfoes", new[] { "StockID" });
            DropTable("dbo.TimeSeriesDataInfoes");
        }
    }
}
