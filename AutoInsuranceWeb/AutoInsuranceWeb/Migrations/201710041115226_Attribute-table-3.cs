namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributetable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportAttributes", "ReportAttributeDefaultValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportAttributes", "ReportAttributeDefaultValue");
        }
    }
}
