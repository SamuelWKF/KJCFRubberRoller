namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maintenances", "sendForRefurbished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Maintenances", "sendForRefurbished");
        }
    }
}
