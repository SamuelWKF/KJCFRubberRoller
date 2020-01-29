using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KJCFRubberRoller.Models
{
    public class Schedule
    {
        [Key]
        [Required]
        [DisplayName("Schedule ID")]
        public int scheduleID { get; set; }

        [Required]
        [DisplayName("Operation Line")]
        public int operationLine { get; set; }

        [Required]
        [DisplayName("Roller ID")]
        public int rollerID { get; set; }
        public virtual RubberRoller RubberRoller { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy,HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Start Date Time")]
        public DateTime? startDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy,HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("End Date Time")]
        public DateTime? endDateTime { get; set; }

        [MaxLength(255)]
        [DisplayName("Product Name")]
        public string product { get; set; }

        [MaxLength(255)]
        [DisplayName("Tinplate Size")]
        public string tinplateSize { get; set; }

        [Required]
        [DisplayName("Quantity")]
        public int quantity { get; set; }

        [Required]
        [DisplayName("Start Mileage")]
        public int startMileage { get; set; }

        [DisplayName("End Mileage")]
        public int endMileage { get; set; }

        [MaxLength(255)]
        [DisplayName("Remark")]
        public string remark { get; set; }
    
        [DisplayName("Status")]
        public int status { get; set; }
        
        public virtual ICollection<BeforeRollerIssueChecklist> BeforeRollerIssueChecklists { get; set; }
        public virtual ICollection<AfterRollerProductionChecklist> AfterRollerProductionChecklists { get; set; }
    }
}