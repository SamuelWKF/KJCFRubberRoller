namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaintenance4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Maintenances", "statusRemark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Maintenances", "statusRemark");
        }
    }
}
