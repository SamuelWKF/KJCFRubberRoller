using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class Staff
    {
        [Key]
        public virtual ApplicationUser id { get; set; }

        [MaxLength(5)]
        [StringLength(5)]
        [DisplayName("Staff ID")]
        [RegularExpression(@"^K\d{4}$", ErrorMessage = "Please enter the correct ID format")]
        public string staffID { get; set; }

        [MaxLength(255)]
        [DisplayName("Name")]
        [RegularExpression(@"^[A-z\-\@\.\, ]{1,}$", ErrorMessage = "Please enter the correct name format.")]
        public string name { get; set; }

        [MaxLength(1)]
        [StringLength(1)]
        [DisplayName("Gender")]
        [RegularExpression(@"^(M|m|F|f)$", ErrorMessage = "Please enter the correct gender format.")]
        public string gender { get; set; }

        [MaxLength(12)]
        [DisplayName("IC Number")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Please enter the correct IC format without \"-\".")]
        public string IC { get; set; }

        [DisplayName("Account Status")]
        public int status { get; set; }
    }
}