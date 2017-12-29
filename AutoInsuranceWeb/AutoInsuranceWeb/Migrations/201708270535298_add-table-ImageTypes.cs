namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableImageTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookMasterViewModels",
                c => new
                    {
                        BookMasterID = c.Int(nullable: false, identity: true),
                        BookTitle = c.String(),
                        StartSNo = c.Long(nullable: false),
                        NumberOfBooks = c.Long(nullable: false),
                        EndSno = c.Long(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.BookMasterID);
            
            CreateTable(
                "dbo.CompanyMasters",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyRemarks = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.DeliveryStatus",
                c => new
                    {
                        DeliveryStatusID = c.Int(nullable: false, identity: true),
                        DeliveryStatusName = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.DeliveryStatusID);
            
            CreateTable(
                "dbo.IssueBooksToDSAs",
                c => new
                    {
                        IssueBookID = c.Int(nullable: false, identity: true),
                        IssueTo = c.String(),
                        IssuedDate = c.DateTime(),
                        SendToAddress = c.String(),
                        Status = c.String(),
                        IssuedBy = c.String(),
                        StartSNo = c.Long(nullable: false),
                        NumberOfBooks = c.Long(nullable: false),
                        EndSno = c.Long(nullable: false),
                        Remarks = c.String(),
                        Location = c.String(),
                        IssuedFrom = c.String(),
                        VehicleCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IssueBookID)
                .ForeignKey("dbo.VehicleCategories", t => t.VehicleCategoryID, cascadeDelete: true)
                .Index(t => t.VehicleCategoryID);
            
            CreateTable(
                "dbo.VehicleCategories",
                c => new
                    {
                        VehicleCategoryID = c.Int(nullable: false, identity: true),
                        VehicleCategoryName = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleCategoryID);
            
            CreateTable(
                "dbo.IssueBooksToROes",
                c => new
                    {
                        IssueBookID = c.Int(nullable: false, identity: true),
                        IssueTo = c.String(),
                        IssuedDate = c.DateTime(),
                        SendToAddress = c.String(),
                        Status = c.String(),
                        IssuedBy = c.String(),
                        StartSNo = c.Long(nullable: false),
                        NumberOfBooks = c.Long(nullable: false),
                        EndSno = c.Long(nullable: false),
                        Remarks = c.String(),
                        Location = c.String(),
                        IssuedFrom = c.String(),
                    })
                .PrimaryKey(t => t.IssueBookID);
            
            CreateTable(
                "dbo.ReportImages",
                c => new
                    {
                        ReportImageID = c.Long(nullable: false, identity: true),
                        ReportNumber = c.Long(nullable: false),
                        ReportID = c.Long(nullable: false),
                        UserId = c.String(),
                        UpdatedDate = c.DateTime(),
                        Imagename = c.String(),
                        ImageType = c.String(),
                        ImagePath = c.String(),
                        ImagePhonePath = c.String(),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientLocation = c.String(),
                    })
                .PrimaryKey(t => t.ReportImageID);
            
            CreateTable(
                "dbo.ReportMasters",
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
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientLocation = c.String(),
                    })
                .PrimaryKey(t => t.ReportID);
            
            CreateTable(
                "dbo.ReportStatus",
                c => new
                    {
                        ReportStatusID = c.Int(nullable: false, identity: true),
                        ReportStatusName = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ReportStatusID);
            
            CreateTable(
                "dbo.ReportVideos",
                c => new
                    {
                        ReportVideosID = c.Long(nullable: false, identity: true),
                        ReportNumber = c.Long(nullable: false),
                        ReportID = c.Long(nullable: false),
                        UserId = c.String(),
                        UpdatedDate = c.DateTime(),
                        Videoname = c.String(),
                        VideoType = c.String(),
                        VideoPath = c.String(),
                        VideoPhonePath = c.String(),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientLocation = c.String(),
                    })
                .PrimaryKey(t => t.ReportVideosID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IssueBooksToDSAs", "VehicleCategoryID", "dbo.VehicleCategories");
            DropIndex("dbo.IssueBooksToDSAs", new[] { "VehicleCategoryID" });
            DropTable("dbo.ReportVideos");
            DropTable("dbo.ReportStatus");
            DropTable("dbo.ReportMasters");
            DropTable("dbo.ReportImages");
            DropTable("dbo.IssueBooksToROes");
            DropTable("dbo.VehicleCategories");
            DropTable("dbo.IssueBooksToDSAs");
            DropTable("dbo.DeliveryStatus");
            DropTable("dbo.CompanyMasters");
            DropTable("dbo.BookMasterViewModels");
        }
    }
}
