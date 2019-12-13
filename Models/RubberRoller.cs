using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class RubberRoller
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public virtual RollerCategory rollerCategoryID { get; set; }
        [Required]
        [MaxLength(10)]
        public string rollerID { get; set; }
        [Required]
        [MaxLength(100)]
        public string type { get; set; }
        [Required]
        [MaxLength(255)]
        public string usage { get; set; }
        [Required]
        [MaxLength(255)]
        public string supplier { get; set; }
        [Required]
        public double diameter { get; set; }
        [Required]
        [MaxLength(255)]
        public string condition { get; set; }
        public DateTime last_usage_date { get; set; }
        [MaxLength(255)]
        public string remark { get; set; }
        [Required]
        [MaxLength(100)]
        public string status { get; set; }      

        // Define relationships
        public virtual ICollection<RollerLocation> RollerLocations { get; set; }
        public virtual ICollection<BeforeRollerIssueChecklist> BeforeRollerIssueChecklists { get; set; }
        public virtual ICollection<AfterRollerProductionChecklist> AfterRollerProductionChecklists { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}