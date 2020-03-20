namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CancoChecklists", "checkedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CancoChecklists", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.CancoChecklists", "verifiedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CancoChecklists", new[] { "checkedBy_Id" });
            DropIndex("dbo.CancoChecklists", new[] { "RubberRoller_id" });
            DropIndex("dbo.CancoChecklists", new[] { "verifiedBy_Id" });
            AddColumn("dbo.RubberRollers", "opening_stock_date", c => c.DateTime());
            DropTable("dbo.CancoChecklists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CancoChecklists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rollerID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        result = c.String(nullable: false, maxLength: 100),
                        scar_issued = c.String(maxLength: 50),
                        remarks = c.String(maxLength: 255),
                        checkedBy_Id = c.String(nullable: false, maxLength: 128),
                        RubberRoller_id = c.Int(),
                        verifiedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id);
            
            DropColumn("dbo.RubberRollers", "opening_stock_date");
            CreateIndex("dbo.CancoChecklists", "verifiedBy_Id");
            CreateIndex("dbo.CancoChecklists", "RubberRoller_id");
            CreateIndex("dbo.CancoChecklists", "checkedBy_Id");
            AddForeignKey("dbo.CancoChecklists", "verifiedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CancoChecklists", "RubberRoller_id", "dbo.RubberRollers", "id");
            AddForeignKey("dbo.CancoChecklists", "checkedBy_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
