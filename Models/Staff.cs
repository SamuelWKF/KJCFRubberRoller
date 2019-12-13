using System;
using System.Collections.Generic;
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
        public string staffID { get; set; }
        [MaxLength(255)]
        public string name { get; set; }
        [MaxLength(1)]
        public string gender { get; set; }
        [MaxLength(12)]
        public string IC { get; set; }
        public int status { get; set; }
    }
}