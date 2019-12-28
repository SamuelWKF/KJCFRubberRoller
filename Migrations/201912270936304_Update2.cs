namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RubberRollers", "shoreHardness", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.RubberRollers", "depthOfGroove", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.RubberRollers", "supplier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RubberRollers", "supplier", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.RubberRollers", "depthOfGroove");
            DropColumn("dbo.RubberRollers", "shoreHardness");
        }
    }
}
