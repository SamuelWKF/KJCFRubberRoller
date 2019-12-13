using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class BeforeRollerIssueChecklist
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public virtual RubberRoller rollerID { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public int operationLine { get; set; }
        [MaxLength(100)]
        [Required]
        public string rollerSH { get; set; }
        [MaxLength(100)]
        [Required]
        public string rollerRoundness { get; set; }
        [MaxLength(100)]
        [Required]
        public string hubsCondition { get; set; }
        [MaxLength(100)]
        [Required]
        public string nutUsed { get; set; }
        [Required]
        public virtual ApplicationUser preparedBy { get; set; }
        public virtual ApplicationUser verifiedBy { get; set; }
    }
}