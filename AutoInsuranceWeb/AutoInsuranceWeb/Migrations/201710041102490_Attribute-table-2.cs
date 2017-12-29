namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributetable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportAttributes", "ReportAttributeDataType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportAttributes", "ReportAttributeDataType");
        }
    }
}
