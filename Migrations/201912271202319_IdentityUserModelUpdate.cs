namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityUserModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Staffs", "id_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Staffs", new[] { "id_Id" });
            AddColumn("dbo.AspNetUsers", "staffID", c => c.String(maxLength: 5));
            AddColumn("dbo.AspNetUsers", "name", c => c.String(maxLength: 255));
            AddColumn("dbo.AspNetUsers", "IC", c => c.String(maxLength: 12));
            AddColumn("dbo.AspNetUsers", "status", c => c.Int(nullable: false));
            DropTable("dbo.Staffs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        staffID = c.String(nullable: false, maxLength: 5),
                        name = c.String(maxLength: 255),
                        IC = c.String(maxLength: 12),
                        status = c.Int(nullable: false),
                        id_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.staffID);
            
            DropColumn("dbo.AspNetUsers", "status");
            DropColumn("dbo.AspNetUsers", "IC");
            DropColumn("dbo.AspNetUsers", "name");
            DropColumn("dbo.AspNetUsers", "staffID");
            CreateIndex("dbo.Staffs", "id_Id");
            AddForeignKey("dbo.Staffs", "id_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
