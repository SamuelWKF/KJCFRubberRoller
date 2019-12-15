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
        [StringLength(255)]
        [DisplayName("Roller Size")]
        public string size { get; set; }

        [Required]
        [MaxLength(255)]
        [StringLength(255)]
        [DisplayName("Roller Category Description")]
        public string description { get; set; }

        [Required(ErrorMessage = "Please enter the minimum amount of roller required")]
        [Range(0, 100, ErrorMessage = "Please enter the proper range")]
        [DisplayName("Minimum Roller Amount")]
        [RegularExpression(@"^[1-9][0-9]?$|^100$", ErrorMessage = "Please enter the correct amount format")]
        public int minAmount { get; set; }

        [MaxLength(255)]
        [StringLength(255)]
        [DisplayName("Remark")]
        public string remark { get; set; }

        [Required(ErrorMessage = "Please select the critical status")]
        [DisplayName("Critical Status")]
        [RegularExpression(@"^[1-3]$", ErrorMessage = "Please select the correct critical status")]
        public int criticalStatus { get; set; }

        public virtual ICollection<RubberRoller> RubberRollers { get; set; }
    }
}