using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class RollerCategory
    {
        [Key]
        [Required]
        public int rollerCategoryID { get; set; }
        [Required]
        [MaxLength(255)]
        public string size { get; set; }
        public string description { get; set; }
        [Required]
        public int minAmount { get; set; }
        [MaxLength(255)]
        public string remark { get; set; }
        [Required]
        public int criticalStatus { get; set; }
        public virtual ICollection<RubberRoller> RubberRollers { get; set; }
    }
}