namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedules", "status");
        }
    }
}
