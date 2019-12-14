using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class RollerCategory
    {
        [Key]
        [Required]
        [DisplayName("Roller Category ID")]
        public int rollerCategoryID { get; set; }
        [Required]
        [MaxLength(255)]
        [DisplayName("Roller Size")]
        public string size { get; set; }
        [Required]
        [DisplayName("Roller Category Description")]
        public string description { get; set; }
        [Required]
        [DisplayName("Minimum Roller Amount")]
        public int minAmount { get; set; }
        [MaxLength(255)]
        [DisplayName("Remark")]
        public string remark { get; set; }
        [Required]
        [DisplayName("Critical Status")]
        public int criticalStatus { get; set; }
        public virtual ICollection<RubberRoller> RubberRollers { get; set; }
    }
}