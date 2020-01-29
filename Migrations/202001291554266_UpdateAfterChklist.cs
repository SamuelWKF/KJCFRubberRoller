namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAfterChklist : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AfterRollerProductionChecklists", "remarks", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AfterRollerProductionChecklists", "remarks", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
