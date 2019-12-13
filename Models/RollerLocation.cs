using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class RollerLocation
    {
        [Key]
        [Required]
        public int rollerLocationID { get; set; }
        public virtual RubberRoller rollerID { get; set; }
        [MaxLength(50)]
        public string location { get; set; }
        public int operationLine { get; set; }
        public DateTime dateTimeIn { get; set; }
        public DateTime dateTimeOut { get; set; }
    }
}