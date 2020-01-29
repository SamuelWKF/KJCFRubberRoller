namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenanceNSchedule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AfterRollerProductionChecklists", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.BeforeRollerIssueChecklists", "RubberRoller_id", "dbo.RubberRollers");
            DropIndex("dbo.AfterRollerProductionChecklists", new[] { "RubberRoller_id" });
            DropIndex("dbo.BeforeRollerIssueChecklists", new[] { "RubberRoller_id" });
            AddColumn("dbo.AfterRollerProductionChecklists", "scheduleID", c => c.Int(nullable: false));
            AddColumn("dbo.BeforeRollerIssueChecklists", "scheduleID", c => c.Int(nullable: false));
            AddColumn("dbo.Maintenances", "title", c => c.String());
            CreateIndex("dbo.AfterRollerProductionChecklists", "scheduleID");
            CreateIndex("dbo.BeforeRollerIssueChecklists", "scheduleID");
            AddForeignKey("dbo.AfterRollerProductionChecklists", "scheduleID", "dbo.Schedules", "scheduleID", cascadeDelete: true);
            AddForeignKey("dbo.BeforeRollerIssueChecklists", "scheduleID", "dbo.Schedules", "scheduleID", cascadeDelete: true);
            DropColumn("dbo.AfterRollerProductionChecklists", "rollerID");
            DropColumn("dbo.AfterRollerProductionChecklists", "operationLine");
            DropColumn("dbo.AfterRollerProductionChecklists", "RubberRoller_id");
            DropColumn("dbo.BeforeRollerIssueChecklists", "rollerID");
            DropColumn("dbo.BeforeRollerIssueChecklists", "operationLine");
            DropColumn("dbo.BeforeRollerIssueChecklists", "RubberRoller_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BeforeRollerIssueChecklists", "RubberRoller_id", c => c.Int());
            AddColumn("dbo.BeforeRollerIssueChecklists", "operationLine", c => c.Int(nullable: false));
            AddColumn("dbo.BeforeRollerIssueChecklists", "rollerID", c => c.Int(nullable: false));
            AddColumn("dbo.AfterRollerProductionChecklists", "RubberRoller_id", c => c.Int());
            AddColumn("dbo.AfterRollerProductionChecklists", "operationLine", c => c.Int(nullable: false));
            AddColumn("dbo.AfterRollerProductionChecklists", "rollerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.BeforeRollerIssueChecklists", "scheduleID", "dbo.Schedules");
            DropForeignKey("dbo.AfterRollerProductionChecklists", "scheduleID", "dbo.Schedules");
            DropIndex("dbo.BeforeRollerIssueChecklists", new[] { "scheduleID" });
            DropIndex("dbo.AfterRollerProductionChecklists", new[] { "scheduleID" });
            DropColumn("dbo.Maintenances", "title");
            DropColumn("dbo.BeforeRollerIssueChecklists", "scheduleID");
            DropColumn("dbo.AfterRollerProductionChecklists", "scheduleID");
            CreateIndex("dbo.BeforeRollerIssueChecklists", "RubberRoller_id");
            CreateIndex("dbo.AfterRollerProductionChecklists", "RubberRoller_id");
            AddForeignKey("dbo.BeforeRollerIssueChecklists", "RubberRoller_id", "dbo.RubberRollers", "id");
            AddForeignKey("dbo.AfterRollerProductionChecklists", "RubberRoller_id", "dbo.RubberRollers", "id");
        }
    }
}
