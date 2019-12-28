namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityUserModelUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "position", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "staffID", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.AspNetUsers", "name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AspNetUsers", "IC", c => c.String(nullable: false, maxLength: 12));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "IC", c => c.String(maxLength: 12));
            AlterColumn("dbo.AspNetUsers", "name", c => c.String(maxLength: 255));
            AlterColumn("dbo.AspNetUsers", "staffID", c => c.String(maxLength: 5));
            DropColumn("dbo.AspNetUsers", "position");
        }
    }
}
