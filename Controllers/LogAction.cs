using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KJCFRubberRoller.Controllers
{
    public class LogAction
    {
        public static void log(string controller, string action, string description, string userID)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Log log = new Log();
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == userID);
                log.dateTime = DateTime.Now;
                log.controller = controller;
                log.action = action;
                log.description = description;
                log.staffID = user;
                _db.logs.Add(log);
                _db.SaveChanges();
            }
        }
    }
}
