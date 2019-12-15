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

        [Route("schedule/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("schedule/create")]
        public ActionResult Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _db.schedules.Add(schedule);
                _db.SaveChanges();
                TempData["saveStatus"] = true;
                TempData["saveStatusMsg"] = "New schedule has been successfully added!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}