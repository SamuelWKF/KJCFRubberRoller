namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenanceNSchedule2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maintenances", "title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Maintenances", "title");
        }
    }
}
