namespace KJCFRubberRoller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using KJCFRubberRoller.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class SeedASPRolesDatabase : DbMigration
    {
        public override void Up()
        {
            SeedRoleTable();
        }

        // Method to seed "Role" table
        private void SeedRoleTable()
        {
            // Declare roles for this application
            string[] app_roles = {
                "QA Staff",
                "Roller PIC",
                "Manager",
                "Executive"
            };

            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            // Loop through roles and create them 
            foreach (string role in app_roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new IdentityRole(role));
                }
            }
        }

        public override void Down()
        {
        }
    }
}
