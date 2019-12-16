using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using KJCFRubberRoller.CustomValidations;

namespace KJCFRubberRoller.Models
{
    public class RubberRoller
    {
        [Key]
        [Required]
        [DisplayName("Roller ID (System)")]
        public int id { get; set; }

        public virtual RollerCategory RollerCategory { get; set; }
        [Required]
        [DisplayName("Roller Category")]
        public int rollerCategoryID { get; set; }

        [Required]
        [MaxLength(10)]
        [DisplayName("Roller ID")]
        [RegularExpression(@"^R\-\d{1,}$", ErrorMessage = "Please enter the correct roller format (R-012)")]
        [UniqueRollerID]
        public string rollerID { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Roller Type")]
        public string type { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Usage")]
        public string usage { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Supplier")]
        public string supplier { get; set; }

        [Required]
        [DisplayName("Diameter")]
        public double diameter { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName(" Condition")]
        public string condition { get; set; }

        [DisplayName("Last Usage Date")]
        public DateTime? last_usage_date { get; set; }

        [MaxLength(255)]
        [DisplayName("Remark")]
        public string remark { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Roller Status")]
        public string status { get; set; }

        // Define relationships
        public virtual ICollection<RollerLocation> RollerLocations { get; set; }
        public virtual ICollection<BeforeRollerIssueChecklist> BeforeRollerIssueChecklists { get; set; }
        public virtual ICollection<AfterRollerProductionChecklist> AfterRollerProductionChecklists { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}