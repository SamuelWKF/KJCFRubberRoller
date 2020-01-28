using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class Maintenance
    {
        [Key]
        [Required]
        [DisplayName("Maintenance ID")]
        public int maintenanceID { get; set; }

        [Required]
        [DisplayName("Roller ID")]
        public int rollerID { get; set; }
        public virtual RubberRoller RubberRoller { get; set; }

        [Required]
        [DisplayName("Reported By")]
        public virtual ApplicationUser reportedBy { get; set; }

        [DisplayName("Verified By")]
        public virtual ApplicationUser verfiedBy { get; set; }

        [Required]
        [DisplayName("Date Reported")]
        public DateTime reportDateTime { get; set; }

        [DisplayName("Date Approved")]
        public DateTime approveDateTime { get; set; }

        [Required]
        [DisplayName("Diameter Core")]
        public int diameterCore { get; set; }

        [Required]
        [DisplayName("Roller Diameter")]
        public double diameterRoller { get; set; }

        [Required]
        [DisplayName("Total Mileage")]
        public int totalMileage { get; set; }

        [DisplayName("Opening Stock Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy,HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime openingStockDate { get; set; }

        [DisplayName("Last Operation Line")]
        public int lastProductionLine { get; set; }
        
        [MaxLength(255)]
        [DisplayName("Reason")]
        public string reason { get; set; }

        [MaxLength(255)]
        [DisplayName("Remark")]
        public string remark { get; set; }

        [DisplayName("New Diameter")]
        public double newDiameter { get; set; }

        [MaxLength(255)]
        [DisplayName("New Shore Hardness")]
        [RegularExpression(@"^[\d\-]+$", ErrorMessage = "Please enter the correct roller format. Example: 40-41")]
        public string newShoreHardness { get; set; }

        [MaxLength(255)]
        [DisplayName("Corrective Action")]
        public string correctiveAction { get; set; }

        [Required]
        [DisplayName("Maintenance Status")]
        public int status { get; set; }
    }
}