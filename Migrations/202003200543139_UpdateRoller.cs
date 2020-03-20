namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRoller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RubberRollers", "isRefurbished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RubberRollers", "isRefurbished");
        }
    }
}
