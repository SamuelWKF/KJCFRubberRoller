using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.CustomValidations
{
    public class UniqueRollerID : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var roller = (RubberRoller)validationContext.ObjectInstance;
            ApplicationDbContext _db = new ApplicationDbContext();
            
            // Check if roller ID exist from DB
            var dbRoller = _db.rubberRollers.Where(r => r.rollerID == roller.rollerID).FirstOrDefault();
            if (dbRoller != null)
            {
                return new ValidationResult("There is already an existing roller with the same roller ID.");
            }
            return ValidationResult.Success;
        }
    }
}