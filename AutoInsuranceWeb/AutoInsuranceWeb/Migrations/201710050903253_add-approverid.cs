namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addapproverid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportMasters", "ApproverID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportMasters", "ApproverID");
        }
    }
}
