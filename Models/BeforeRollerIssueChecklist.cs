using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KJCFRubberRoller.Models
{
    public class BeforeRollerIssueChecklist
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
        public DateTime? dateTime { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Shore Hardness")]
        public string rollerSH { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Roller Roundness")]
        public string rollerRoundness { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Hubs Condition")]
        public string hubsCondition { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Nut Used")]
        public string nutUsed { get; set; }

        [Required]
        [DisplayName("Prepared By")]
        public virtual ApplicationUser preparedBy { get; set; }

        [DisplayName("Verified By")]
        public virtual ApplicationUser verifiedBy { get; set; }
    }
}