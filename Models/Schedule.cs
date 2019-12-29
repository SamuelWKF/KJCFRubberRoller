﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KJCFRubberRoller.Models
{
    public class Schedule
    {
        [Key]
        [Required]
        [DisplayName("Schedule ID")]
        public int scheduleID { get; set; }

        [Required]
        [DisplayName("Operation Line")]
        public int operationLine { get; set; }

        [Required]
        [DisplayName("Roller ID")]
        public int rollerID { get; set; }
        public virtual RubberRoller RubberRoller { get; set; }

        [Required]
        [DisplayName("Start Date Time")]
        public DateTime? startDateTime { get; set; }

        [Required]
        [DisplayName("End Date Time")]
        public DateTime endDateTime { get; set; }

        [MaxLength(255)]
        [DisplayName("Product Name")]
        public string product { get; set; }

        [MaxLength(255)]
        [DisplayName("Tinplate Size")]
        public string tinplateSize { get; set; }

        [Required]
        [DisplayName("Quantity")]
        public int quantity { get; set; }

        [Required]
        [DisplayName("Start Mileage")]
        public int startMileage { get; set; }

        [Required]
        [DisplayName("End Mileage")]
        public int endMileage { get; set; }

        [MaxLength(255)]
        [DisplayName("Remark")]
        public string remark { get; set; }
    }
}