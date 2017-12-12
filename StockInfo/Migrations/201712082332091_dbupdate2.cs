namespace StockInfo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSeriesDataInfoes", "TimeStamp", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSeriesDataInfoes", "TimeStamp");
        }
    }
}
