namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedule1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schedules", "endDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "endDateTime", c => c.DateTime(nullable: false));
        }
    }
}
