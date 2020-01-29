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
        [MaxLength(15)]
        [DisplayName("Roller ID")]
        [RegularExpression(@"^R\-\d{1,}[\s\w\(\)\/]+$", ErrorMessage = "Please enter the correct roller format. Example: R-012, R-012 (P/S)")]
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
        [MaxLength(100)]
        [DisplayName("Shore Hardness")]
        [RegularExpression(@"^[\d\-]+$", ErrorMessage = "Please enter the correct roller format. Example: 40-41")]
        public string shoreHardness { get; set; }

        [Required]
        [DisplayName("Diameter")]
        public double diameter { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Depth of Roller Groove")]
        [RegularExpression(@"^[\d\-]+$", ErrorMessage = "Please enter the correct roller format. Example: 40-41")]
        public string depthOfGroove { get; set; }

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
        public virtual ICollection<Maintenance> Maintenances { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}