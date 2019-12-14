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
        [DisplayName("Checklist Id")]
        public int id { get; set; }
        [Required]
        [DisplayName("Roller Id")]
        public virtual RubberRoller rollerID { get; set; }
        [Required]
        [DisplayName("Date and Time")]
        public DateTime dateTime { get; set; }
        [Required]
        [DisplayName("Operation Time")]
        public int operationLine { get; set; }
        [MaxLength(100)]
        [Required]
        [DisplayName("Roller Appearance")]
        public string rollerAppearance { get; set; }
        [MaxLength(100)]
        [Required]
        [DisplayName("Roller Sent To")]
        public string rollerSendTo { get; set; }
        [MaxLength(255)]
        [Required]
        [DisplayName("Remark")]
        public string remarks { get; set; }
        [Required]
        [DisplayName("Prepared By")]
        public virtual ApplicationUser preparedBy { get; set; }
        [DisplayName("Verified By")]
        public virtual ApplicationUser verifiedBy { get; set; }
    }
}