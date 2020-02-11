using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KJCFRubberRoller.Models;

namespace KJCFRubberRoller.Controllers.Classes
{
    public class CentralUtilities
    {
        public static bool UpdateRollerLocation(RubberRoller rubberRoller, string location)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.id == rubberRoller.id);
            if (rubber == null)
                return false;

            // Update roller location
            RollerLocation currentLocation = rubber.RollerLocations.LastOrDefault();
            if (currentLocation != null)
                currentLocation.dateTimeOut = DateTime.Now;

            RollerLocation rollerLocation = new RollerLocation();
            rollerLocation.dateTimeIn = DateTime.Now;
            rollerLocation.rollerID = rubber.id;
            rollerLocation.RubberRoller = rubber;
            rollerLocation.location = location;
            rollerLocation.operationLine = 0;

            // Add new records
            _db.rollerLocations.Add(rollerLocation);
            var result = _db.SaveChanges();
            return result > 0 ? true : false;
        }
    }
}