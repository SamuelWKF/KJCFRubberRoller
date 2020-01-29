namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenanceNSchedule1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Maintenances", "title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Maintenances", "title", c => c.String(nullable: false));
        }
    }
}
