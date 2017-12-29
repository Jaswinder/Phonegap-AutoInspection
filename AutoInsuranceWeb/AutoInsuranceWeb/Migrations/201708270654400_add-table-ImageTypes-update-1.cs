namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableImageTypesupdate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReportImages", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.ReportImages", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.ReportMasters", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.ReportMasters", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.ReportVideos", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
            AlterColumn("dbo.ReportVideos", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReportVideos", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ReportVideos", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ReportMasters", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ReportMasters", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ReportImages", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ReportImages", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
