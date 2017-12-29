namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserLoactionsTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportMasterPreviews",
                c => new
                    {
                        ReportID = c.Long(nullable: false, identity: true),
                        ReportNumber = c.Long(nullable: false),
                        UserId = c.String(),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        VehicleCategoryID = c.Int(nullable: false),
                        VehicleNumber = c.String(),
                        ChassisNumber = c.String(),
                        BookNumber = c.Long(nullable: false),
                        Comment = c.String(),
                        CompanyID = c.String(),
                        CompanyName = c.String(),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 9),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 9),
                        ClientLocation = c.String(),
                        MobileNumber = c.String(),
                    })
                .PrimaryKey(t => t.ReportID);
            
            CreateTable(
                "dbo.UserLocationHistories",
                c => new
                    {
                        UserLocationID = c.Int(nullable: false, identity: true),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 9),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 9),
                        UserId = c.String(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserLocationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserLocationHistories");
            DropTable("dbo.ReportMasterPreviews");
        }
    }
}
