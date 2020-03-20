namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RollerLocations", "location", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RollerLocations", "location", c => c.String(maxLength: 50));
        }
    }
}
