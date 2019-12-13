using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Models
{
    public class Log
    {
        [Key]
        [Required]
        public int logID { get; set; }
        [Required]
        public virtual ApplicationUser staffID { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        [MaxLength(255)]
        public string action { get; set; }
    }
}