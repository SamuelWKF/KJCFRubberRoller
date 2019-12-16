using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KJCFRubberRoller.Models
{
    public class CancoChecklist
    {
        [Key]
        [Required]
        [DisplayName("Checklist ID")]
        public int id { get; set; }

        [Required]
        [DisplayName("Roller ID")]
        public int rollerID { get; set; }
        public virtual RubberRoller RubberRoller { get; set; }

        [Required]
        [DisplayName("Date")]
        public DateTime date { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Result")]
        public string result { get; set; }

        [MaxLength(50)]
        [DisplayName("SCAR Issued")]
        public string scar_issued { get; set; }

        [MaxLength(255)]
        [DisplayName("Remarks")]
        public string remarks { get; set; }

        [Required]
        [DisplayName("Checked By")]
        public virtual ApplicationUser checkedBy { get; set; }

        [DisplayName("Verified By")]
        public virtual ApplicationUser verifiedBy { get; set; }
    }
}