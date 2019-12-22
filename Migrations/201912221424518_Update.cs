namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RubberRollers", "rollerID", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RubberRollers", "rollerID", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
