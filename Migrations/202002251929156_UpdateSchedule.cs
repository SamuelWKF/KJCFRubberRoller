namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedule : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schedules", "product", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "product", c => c.String(maxLength: 255));
        }
    }
}
