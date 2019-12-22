using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class Log
    {
        [Key]
        [Required]
        [DisplayName("Log ID")]
        public int logID { get; set; }

        [Required]
        [DisplayName("Staff ID")]
        public virtual ApplicationUser staffID { get; set; }

        [Required]
        [DisplayName("Date Time")]
        public DateTime dateTime { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Controller")]
        public string controller { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Action")]
        public string action { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Description")]
        public string description { get; set; }
    }
}