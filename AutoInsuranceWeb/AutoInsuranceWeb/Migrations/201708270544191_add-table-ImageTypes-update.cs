namespace AutoInsuranceWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableImageTypesupdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageTypes",
                c => new
                    {
                        ImageTypeID = c.Int(nullable: false, identity: true),
                        ImageTypeName = c.String(),
                        ImageTypeRemarks = c.String(),
                        Code = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ImageTypeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImageTypes");
        }
    }
}
