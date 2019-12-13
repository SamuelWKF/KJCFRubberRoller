using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class Schedule
    {
        [Key]
        [Required]
        public int scheduleID { get; set; }
        [Required]
        public int operationLine { get; set; }
        [Required]
        public virtual RubberRoller rollerID { get; set; }
        [Required]
        public DateTime startDateTime { get; set; }
        [Required]
        public DateTime endDateTime { get; set; }
        [MaxLength(255)]
        public string product { get; set; }
        [MaxLength(255)]
        public string tinplateSize { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public int startMileage { get; set; }
        [Required]
        public int endMileage { get; set; }
        [MaxLength(255)]
        public string remark { get; set; }
    }
}