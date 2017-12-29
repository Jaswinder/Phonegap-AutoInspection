namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCourierCompanyMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourierCompanyMasters",
                c => new
                    {
                        CourierCompanyID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyRemarks = c.String(),
                        ContactNumber = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.CourierCompanyID);
            
            AddColumn("dbo.IssueBooksToDSAs", "CourierCompanyID", c => c.Int(nullable: false));
            AddColumn("dbo.IssueBooksToDSAs", "TrackingID", c => c.String());
            AddColumn("dbo.IssueBooksToROes", "CourierCompanyID", c => c.Int(nullable: false));
            AddColumn("dbo.IssueBooksToROes", "TrackingID", c => c.String());
            CreateIndex("dbo.IssueBooksToDSAs", "CourierCompanyID");
            CreateIndex("dbo.IssueBooksToROes", "CourierCompanyID");
            AddForeignKey("dbo.IssueBooksToDSAs", "CourierCompanyID", "dbo.CourierCompanyMasters", "CourierCompanyID", cascadeDelete: true);
            AddForeignKey("dbo.IssueBooksToROes", "CourierCompanyID", "dbo.CourierCompanyMasters", "CourierCompanyID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IssueBooksToROes", "CourierCompanyID", "dbo.CourierCompanyMasters");
            DropForeignKey("dbo.IssueBooksToDSAs", "CourierCompanyID", "dbo.CourierCompanyMasters");
            DropIndex("dbo.IssueBooksToROes", new[] { "CourierCompanyID" });
            DropIndex("dbo.IssueBooksToDSAs", new[] { "CourierCompanyID" });
            DropColumn("dbo.IssueBooksToROes", "TrackingID");
            DropColumn("dbo.IssueBooksToROes", "CourierCompanyID");
            DropColumn("dbo.IssueBooksToDSAs", "TrackingID");
            DropColumn("dbo.IssueBooksToDSAs", "CourierCompanyID");
            DropTable("dbo.CourierCompanyMasters");
        }
    }
}
