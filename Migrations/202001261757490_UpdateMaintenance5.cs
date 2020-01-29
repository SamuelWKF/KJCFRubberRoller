namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenance5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Maintenances", "diameterCore", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Maintenances", "diameterCore", c => c.Int(nullable: false));
        }
    }
}
