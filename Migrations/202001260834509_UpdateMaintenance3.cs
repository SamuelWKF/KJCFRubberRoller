namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenance3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Maintenances", "approveDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Maintenances", "approveDateTime", c => c.DateTime(nullable: false));
        }
    }
}
