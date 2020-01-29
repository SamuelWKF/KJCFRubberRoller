namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenance1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Maintenances", "newShoreHardness", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Maintenances", "newShoreHardness", c => c.String(maxLength: 255));
        }
    }
}
