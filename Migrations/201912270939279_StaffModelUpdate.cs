namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Staffs", "gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Staffs", "gender", c => c.String(maxLength: 1));
        }
    }
}
