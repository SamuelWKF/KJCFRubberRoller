using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KJCFRubberRoller.Models
{
    public class AfterRollerProductionChecklist
    {
        [Key]
        [Required]
        [DisplayName("Checklist ID")]
        public int id { get; set; }

        [Required]
        [DisplayName("Schedule ID")]
        public int scheduleID { get; set; }
        public virtual Schedule Schedule { get; set; }

        [Required]
        [DisplayName("Date Time")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? dateTime { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Roller Appearance")]
        public string rollerAppearance { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Roller Sent To")]
        public string rollerSendTo { get; set; }

        [MaxLength(255)]
        [DisplayName("Remark")]
        public string remarks { get; set; }

        [Required]
        [DisplayName("Prepared By")]
        public virtual ApplicationUser preparedBy { get; set; }

        [DisplayName("Verified By")]
        public virtual ApplicationUser verifiedBy { get; set; }
    }
}