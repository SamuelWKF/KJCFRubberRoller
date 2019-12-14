using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    public class ScheduleController : Controller
    {
        private ApplicationDbContext _db;

        // Constructor
        public ScheduleController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }
        // GET: Schedule
        public ActionResult Index()
        {
            return View();
        }
    }
}