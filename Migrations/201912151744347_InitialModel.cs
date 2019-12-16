namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AfterRollerProductionChecklists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rollerID = c.Int(nullable: false),
                        dateTime = c.DateTime(nullable: false),
                        operationLine = c.Int(nullable: false),
                        rollerAppearance = c.String(nullable: false, maxLength: 100),
                        rollerSendTo = c.String(nullable: false, maxLength: 100),
                        remarks = c.String(nullable: false, maxLength: 255),
                        preparedBy_Id = c.String(nullable: false, maxLength: 128),
                        RubberRoller_id = c.Int(),
                        verifiedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.preparedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.RubberRollers", t => t.RubberRoller_id)
                .ForeignKey("dbo.AspNetUsers", t => t.verifiedBy_Id)
                .Index(t => t.preparedBy_Id)
                .Index(t => t.RubberRoller_id)
                .Index(t => t.verifiedBy_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RubberRollers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rollerCategoryID = c.Int(nullable: false),
                        rollerID = c.String(nullable: false, maxLength: 10),
                        type = c.String(nullable: false, maxLength: 100),
                        usage = c.String(nullable: false, maxLength: 255),
                        supplier = c.String(nullable: false, maxLength: 255),
                        diameter = c.Double(nullable: false),
                        condition = c.String(nullable: false, maxLength: 255),
                        last_usage_date = c.DateTime(),
                        remark = c.String(maxLength: 255),
                        status = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.RollerCategories", t => t.rollerCategoryID, cascadeDelete: true)
                .Index(t => t.rollerCategoryID);
            
            CreateTable(
                "dbo.BeforeRollerIssueChecklists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rollerID = c.Int(nullable: false),
                        dateTime = c.DateTime(nullable: false),
                        operationLine = c.Int(nullable: false),
                        rollerSH = c.String(nullable: false, maxLength: 100),
                        rollerRoundness = c.String(nullable: false, maxLength: 100),
                        hubsCondition = c.String(nullable: false, maxLength: 100),
                        nutUsed = c.String(nullable: false, maxLength: 100),
                        preparedBy_Id = c.String(nullable: false, maxLength: 128),
                        RubberRoller_id = c.Int(),
                        verifiedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.preparedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.RubberRollers", t => t.RubberRoller_id)
                .ForeignKey("dbo.AspNetUsers", t => t.verifiedBy_Id)
                .Index(t => t.preparedBy_Id)
                .Index(t => t.RubberRoller_id)
                .Index(t => t.verifiedBy_Id);
            
            CreateTable(
                "dbo.Maintenances",
                c => new
                    {
                        maintenanceID = c.Int(nullable: false, identity: true),
                        rollerID = c.Int(nullable: false),
                        reportDateTime = c.DateTime(nullable: false),
                        approveDateTime = c.DateTime(nullable: false),
                        diameterCore = c.Int(nullable: false),
                        diameterRoller = c.Double(nullable: false),
                        totalMileage = c.Int(nullable: false),
                        openingStockDate = c.DateTime(nullable: false),
                        lastProductionLine = c.Int(nullable: false),
                        reason = c.String(maxLength: 255),
                        remark = c.String(maxLength: 255),
                        newDiameter = c.Double(nullable: false),
                        newShoreHardness = c.String(maxLength: 255),
                        correctiveAction = c.String(maxLength: 255),
                        status = c.Int(nullable: false),
                        reportedBy_Id = c.String(nullable: false, maxLength: 128),
                        RubberRoller_id = c.Int(),
                        verfiedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.maintenanceID)
                .ForeignKey("dbo.AspNetUsers", t => t.reportedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.RubberRollers", t => t.RubberRoller_id)
                .ForeignKey("dbo.AspNetUsers", t => t.verfiedBy_Id)
                .Index(t => t.reportedBy_Id)
                .Index(t => t.RubberRoller_id)
                .Index(t => t.verfiedBy_Id);
            
            CreateTable(
                "dbo.RollerCategories",
                c => new
                    {
                        rollerCategoryID = c.Int(nullable: false, identity: true),
                        size = c.String(nullable: false, maxLength: 255),
                        description = c.String(nullable: false, maxLength: 255),
                        minAmount = c.Int(nullable: false),
                        remark = c.String(maxLength: 255),
                        criticalStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.rollerCategoryID);
            
            CreateTable(
                "dbo.RollerLocations",
                c => new
                    {
                        rollerLocationID = c.Int(nullable: false, identity: true),
                        rollerID = c.Int(nullable: false),
                        location = c.String(maxLength: 50),
                        operationLine = c.Int(nullable: false),
                        dateTimeIn = c.DateTime(nullable: false),
                        dateTimeOut = c.DateTime(nullable: false),
                        RubberRoller_id = c.Int(),
                    })
                .PrimaryKey(t => t.rollerLocationID)
                .ForeignKey("dbo.RubberRollers", t => t.RubberRoller_id)
                .Index(t => t.RubberRoller_id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        scheduleID = c.Int(nullable: false, identity: true),
                        operationLine = c.Int(nullable: false),
                        rollerID = c.Int(nullable: false),
                        startDateTime = c.DateTime(nullable: false),
                        endDateTime = c.DateTime(nullable: false),
                        product = c.String(maxLength: 255),
                        tinplateSize = c.String(maxLength: 255),
                        quantity = c.Int(nullable: false),
                        startMileage = c.Int(nullable: false),
                        endMileage = c.Int(nullable: false),
                        remark = c.String(maxLength: 255),
                        RubberRoller_id = c.Int(),
                    })
                .PrimaryKey(t => t.scheduleID)
                .ForeignKey("dbo.RubberRollers", t => t.RubberRoller_id)
                .Index(t => t.RubberRoller_id);
            
            CreateTable(
                "dbo.CancoChecklists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rollerID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        result = c.String(nullable: false, maxLength: 100),
                        scar_issued = c.String(maxLength: 50),
                        remarks = c.String(maxLength: 255),
                        checkedBy_Id = c.String(nullable: false, maxLength: 128),
                        RubberRoller_id = c.Int(),
                        verifiedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.checkedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.RubberRollers", t => t.RubberRoller_id)
                .ForeignKey("dbo.AspNetUsers", t => t.verifiedBy_Id)
                .Index(t => t.checkedBy_Id)
                .Index(t => t.RubberRoller_id)
                .Index(t => t.verifiedBy_Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        logID = c.Int(nullable: false, identity: true),
                        dateTime = c.DateTime(nullable: false),
                        action = c.String(nullable: false, maxLength: 255),
                        staffID_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.logID)
                .ForeignKey("dbo.AspNetUsers", t => t.staffID_Id, cascadeDelete: true)
                .Index(t => t.staffID_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        staffID = c.String(nullable: false, maxLength: 5),
                        name = c.String(maxLength: 255),
                        gender = c.String(maxLength: 1),
                        IC = c.String(maxLength: 12),
                        status = c.Int(nullable: false),
                        id_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.staffID)
                .ForeignKey("dbo.AspNetUsers", t => t.id_Id)
                .Index(t => t.id_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "id_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Logs", "staffID_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CancoChecklists", "verifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CancoChecklists", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.CancoChecklists", "checkedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AfterRollerProductionChecklists", "verifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Schedules", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.RollerLocations", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.RubberRollers", "rollerCategoryID", "dbo.RollerCategories");
            DropForeignKey("dbo.Maintenances", "verfiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Maintenances", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.Maintenances", "reportedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BeforeRollerIssueChecklists", "verifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BeforeRollerIssueChecklists", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.BeforeRollerIssueChecklists", "preparedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AfterRollerProductionChecklists", "RubberRoller_id", "dbo.RubberRollers");
            DropForeignKey("dbo.AfterRollerProductionChecklists", "preparedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Staffs", new[] { "id_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Logs", new[] { "staffID_Id" });
            DropIndex("dbo.CancoChecklists", new[] { "verifiedBy_Id" });
            DropIndex("dbo.CancoChecklists", new[] { "RubberRoller_id" });
            DropIndex("dbo.CancoChecklists", new[] { "checkedBy_Id" });
            DropIndex("dbo.Schedules", new[] { "RubberRoller_id" });
            DropIndex("dbo.RollerLocations", new[] { "RubberRoller_id" });
            DropIndex("dbo.Maintenances", new[] { "verfiedBy_Id" });
            DropIndex("dbo.Maintenances", new[] { "RubberRoller_id" });
            DropIndex("dbo.Maintenances", new[] { "reportedBy_Id" });
            DropIndex("dbo.BeforeRollerIssueChecklists", new[] { "verifiedBy_Id" });
            DropIndex("dbo.BeforeRollerIssueChecklists", new[] { "RubberRoller_id" });
            DropIndex("dbo.BeforeRollerIssueChecklists", new[] { "preparedBy_Id" });
            DropIndex("dbo.RubberRollers", new[] { "rollerCategoryID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AfterRollerProductionChecklists", new[] { "verifiedBy_Id" });
            DropIndex("dbo.AfterRollerProductionChecklists", new[] { "RubberRoller_id" });
            DropIndex("dbo.AfterRollerProductionChecklists", new[] { "preparedBy_Id" });
            DropTable("dbo.Staffs");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Logs");
            DropTable("dbo.CancoChecklists");
            DropTable("dbo.Schedules");
            DropTable("dbo.RollerLocations");
            DropTable("dbo.RollerCategories");
            DropTable("dbo.Maintenances");
            DropTable("dbo.BeforeRollerIssueChecklists");
            DropTable("dbo.RubberRollers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AfterRollerProductionChecklists");
        }
    }
}
