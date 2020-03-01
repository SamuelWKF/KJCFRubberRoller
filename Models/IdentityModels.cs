using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [MaxLength(5)]
        [StringLength(5)]
        [DisplayName("Staff ID")]
        [RegularExpression(@"[kK]\d+", ErrorMessage = "Staff Id must start with K & follow by maximum 4 digits")]
        public string staffID { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Name")]
        [RegularExpression(@"^[A-z\-\@\.\, ]{1,}$", ErrorMessage = "Please enter the correct name format.")]
        public string name { get; set; }

        [Required]
        [MaxLength(12)]
        [DisplayName("IC Number")]
        [RegularExpression(@"^([0-9][0-9])((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))([0-9][0-9])([0-9][0-9][0-9][0-9])$", ErrorMessage = "Please enter the correct IC format without \"-\". E.g. 651212015591")]
        public string IC { get; set; }

        [Required]
        [DisplayName("Position")]
        [RegularExpression(@"^[1-4]$", ErrorMessage = "Please select the correct position.")]
        public int position { get; set; }

        [Required]
        [DisplayName("Account Status")]
        public int status { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Register all the database models
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