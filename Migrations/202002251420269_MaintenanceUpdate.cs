namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaintenanceUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maintenances", "imagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Maintenances", "imagePath");
        }
    }
}
