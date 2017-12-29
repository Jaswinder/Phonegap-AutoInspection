namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportAttributes",
                c => new
                    {
                        ReportAttributeID = c.Long(nullable: false, identity: true),
                        ReportAttributeName = c.String(),
                        ReportAttributeType = c.String(),
                        ReportID = c.Long(nullable: false),
                        VehicleCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportAttributeID)
                .ForeignKey("dbo.VehicleCategories", t => t.VehicleCategoryID, cascadeDelete: true)
                .Index(t => t.VehicleCategoryID);
            
            CreateTable(
                "dbo.ReportAttributeValues",
                c => new
                    {
                        ReportAttributeValueID = c.Long(nullable: false, identity: true),
                        ReportAttributeValueName = c.String(),
                        ReportAttributeID = c.Int(nullable: false),
                        ReportAttribute_ReportAttributeID = c.Long(),
                    })
                .PrimaryKey(t => t.ReportAttributeValueID)
                .ForeignKey("dbo.ReportAttributes", t => t.ReportAttribute_ReportAttributeID)
                .Index(t => t.ReportAttribute_ReportAttributeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportAttributes", "VehicleCategoryID", "dbo.VehicleCategories");
            DropForeignKey("dbo.ReportAttributeValues", "ReportAttribute_ReportAttributeID", "dbo.ReportAttributes");
            DropIndex("dbo.ReportAttributeValues", new[] { "ReportAttribute_ReportAttributeID" });
            DropIndex("dbo.ReportAttributes", new[] { "VehicleCategoryID" });
            DropTable("dbo.ReportAttributeValues");
            DropTable("dbo.ReportAttributes");
        }
    }
}
