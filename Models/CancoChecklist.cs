using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KJCFRubberRoller.Models
{
    public class CancoChecklist
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public virtual RubberRoller rollerID { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        [MaxLength(100)]
        public string result { get; set; }
        [MaxLength(50)]
        public string scar_issued { get; set; }
        [MaxLength(255)]
        public string remarks { get; set; }
        [Required]
        public virtual ApplicationUser checkedBy { get; set; }
        public virtual ApplicationUser verifiedBy { get; set; }
    }
}