namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BeforeRollerIssueChecklists", "verifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AfterRollerProductionChecklists", "verifiedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AfterRollerProductionChecklists", new[] { "verifiedBy_Id" });
            DropIndex("dbo.BeforeRollerIssueChecklists", new[] { "verifiedBy_Id" });
            DropColumn("dbo.AfterRollerProductionChecklists", "verifiedBy_Id");
            DropColumn("dbo.BeforeRollerIssueChecklists", "verifiedBy_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BeforeRollerIssueChecklists", "verifiedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AfterRollerProductionChecklists", "verifiedBy_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.BeforeRollerIssueChecklists", "verifiedBy_Id");
            CreateIndex("dbo.AfterRollerProductionChecklists", "verifiedBy_Id");
            AddForeignKey("dbo.AfterRollerProductionChecklists", "verifiedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.BeforeRollerIssueChecklists", "verifiedBy_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
