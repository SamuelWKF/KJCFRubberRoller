using KJCFRubberRoller.Models;
using Microsoft.AspNet.Identity;
using PagedList;
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
        private string _controllerName = "Schedule";

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


        public ActionResult Index(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested Schedule-Index webpage", User.Identity.GetUserId());
            List<Schedule> schedules = _db.schedules.ToList();
            return View(schedules.ToPagedList(i ?? 1, 20));
        }


        public ActionResult Create()
        {
            LogAction.log(this._controllerName, "GET", "Requested Schedule-Create webpage", User.Identity.GetUserId());
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Schedule schedule)
        {
            try
            {
                _db.schedules.Add(schedule);
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "New rubber roller schedule has been successfully added!";
                    LogAction.log(this._controllerName, "POST", "Added new schedule record", User.Identity.GetUserId());
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller schedule has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}