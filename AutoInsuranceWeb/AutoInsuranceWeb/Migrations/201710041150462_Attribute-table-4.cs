namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributetable4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReportAttributeValues", "ReportAttribute_ReportAttributeID", "dbo.ReportAttributes");
            DropIndex("dbo.ReportAttributeValues", new[] { "ReportAttribute_ReportAttributeID" });
            DropColumn("dbo.ReportAttributeValues", "ReportAttributeID");
            RenameColumn(table: "dbo.ReportAttributeValues", name: "ReportAttribute_ReportAttributeID", newName: "ReportAttributeID");
            AddColumn("dbo.ReportAttributeValues", "ReportID", c => c.Long(nullable: false));
            AlterColumn("dbo.ReportAttributeValues", "ReportAttributeID", c => c.Long(nullable: false));
            AlterColumn("dbo.ReportAttributeValues", "ReportAttributeID", c => c.Long(nullable: false));
            CreateIndex("dbo.ReportAttributeValues", "ReportAttributeID");
            AddForeignKey("dbo.ReportAttributeValues", "ReportAttributeID", "dbo.ReportAttributes", "ReportAttributeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportAttributeValues", "ReportAttributeID", "dbo.ReportAttributes");
            DropIndex("dbo.ReportAttributeValues", new[] { "ReportAttributeID" });
            AlterColumn("dbo.ReportAttributeValues", "ReportAttributeID", c => c.Long());
            AlterColumn("dbo.ReportAttributeValues", "ReportAttributeID", c => c.Int(nullable: false));
            DropColumn("dbo.ReportAttributeValues", "ReportID");
            RenameColumn(table: "dbo.ReportAttributeValues", name: "ReportAttributeID", newName: "ReportAttribute_ReportAttributeID");
            AddColumn("dbo.ReportAttributeValues", "ReportAttributeID", c => c.Int(nullable: false));
            CreateIndex("dbo.ReportAttributeValues", "ReportAttribute_ReportAttributeID");
            AddForeignKey("dbo.ReportAttributeValues", "ReportAttribute_ReportAttributeID", "dbo.ReportAttributes", "ReportAttributeID");
        }
    }
}
