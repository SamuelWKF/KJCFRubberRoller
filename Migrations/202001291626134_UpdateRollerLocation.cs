namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRollerLocation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RollerLocations", "dateTimeIn", c => c.DateTime());
            AlterColumn("dbo.RollerLocations", "dateTimeOut", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RollerLocations", "dateTimeOut", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RollerLocations", "dateTimeIn", c => c.DateTime(nullable: false));
        }
    }
}
