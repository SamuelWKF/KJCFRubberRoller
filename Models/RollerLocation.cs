using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class RollerLocation
    {
        [Key]
        [Required]
        [DisplayName("Roller Location ID")]
        public int rollerLocationID { get; set; }

        [Required]
        [DisplayName("Rubber Roller")]
        public int rollerID { get; set; }
        public virtual RubberRoller RubberRoller { get; set; }

        [MaxLength(50)]
        [DisplayName("Roller Location")]
        public string location { get; set; }

        [DisplayName("Operation Line")]
        [RegularExpression(@"^[1-9][0-9]?$|^100$", ErrorMessage = "Please enter the correct operation line format")]
        public int operationLine { get; set; }

        [DisplayName("Date Time In")]
        public DateTime dateTimeIn { get; set; }

        [DisplayName("Date Time Out")]
        public DateTime dateTimeOut { get; set; }
    }
}