using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class Maintenance
    {
        [Key]
        [Required]
        public int maintenanceID { get; set; }
        [Required]
        public virtual RubberRoller rollerID { get; set; }
        public virtual ApplicationUser reportedBy { get; set; }
        public virtual ApplicationUser verfiedBy { get; set; }
        [Required]
        public DateTime reportDateTime { get; set; }
        public DateTime approveDateTime { get; set; }
        [Required]
        public int diameterCore { get; set; }
        [Required]
        public double diameterRoller { get; set; }
        [Required]
        public int totalMileage { get; set; }
        public DateTime openingStockDate { get; set; }
        public int lastProductionLine { get; set; }
        [MaxLength(255)]
        public string reason { get; set; }
        [MaxLength(255)]
        public string remark { get; set; }
        public double newDiameter { get; set; }
        [MaxLength(255)]
        public string newShoreHardness { get; set; }
        [MaxLength(255)]
        public string correctiveAction { get; set; }
        [Required]
        public int status { get; set; }
    }
}