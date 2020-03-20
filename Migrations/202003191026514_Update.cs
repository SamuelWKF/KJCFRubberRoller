namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Maintenances", "lastProductionLine", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Maintenances", "lastProductionLine", c => c.Int(nullable: false));
        }
    }
}
