namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "controller", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Logs", "description", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "description");
            DropColumn("dbo.Logs", "controller");
        }
    }
}
