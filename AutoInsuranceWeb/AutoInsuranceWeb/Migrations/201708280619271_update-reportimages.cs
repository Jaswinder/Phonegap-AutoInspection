namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatereportimages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportImages", "ImagePathStemped", c => c.String());
            AddColumn("dbo.ReportImages", "ImagePathThumb", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportImages", "ImagePathThumb");
            DropColumn("dbo.ReportImages", "ImagePathStemped");
        }
    }
}
