using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KJCFRubberRoller.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Register all the database models
        public DbSet<Staff> staffs { get; set; }
        public DbSet<Log> logs { get; set; }
        public DbSet<RollerCategory> rollerCategories { get; set; }
        public DbSet<RubberRoller> rubberRollers { get; set; }
        public DbSet<RollerLocation> rollerLocations { get; set; }
        public DbSet<BeforeRollerIssueChecklist> beforeRollerIssueChecklists { get; set; }
        public DbSet<AfterRollerProductionChecklist> afterRollerProductionChecklists { get; set; }
        public DbSet<Schedule> schedules { get; set; }
        public DbSet<Maintenance> maintenances { get; set; }
        public DbSet<CancoChecklist> cancoChecklists { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}