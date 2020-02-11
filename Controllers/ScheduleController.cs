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

        // Displays a list of current active operation
        public ActionResult ActiveOperation(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested Schedule-ActiveOperation webpage", User.Identity.GetUserId());
            List<Schedule> schedules = _db.schedules.Where(s => s.status == ScheduleStatus.ACTIVE).ToList();
            return View(schedules.ToPagedList(i ?? 1, 20));
        }

        // Displays a list of completed operation
        public ActionResult CompletedOperation(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested Schedule-CompletedOperation webpage", User.Identity.GetUserId());
            List<Schedule> schedules = _db.schedules.Where(s => s.status == ScheduleStatus.COMPLETED).ToList();
            return View(schedules.ToPagedList(i ?? 1, 20));
        }

        // Display search roller form - 1st step of adding a new schedule/operation
        public ActionResult CreateSearch()
        {
            // Retrieve rollers
            var rubberRollers = _db.rubberRollers
                .AsEnumerable()
                .Select(s => new
                {
                    ID = s.id,
                    s.rollerID
                }).ToList();

            ViewData["rollerList"] = new SelectList(rubberRollers, "ID", "rollerID");
            LogAction.log(this._controllerName, "GET", "Requested Schedule-CreateSearch webpage", User.Identity.GetUserId());
            return View("SearchRoller");
        }

        // Display create schedule record after selecting rubber roller
        [Route("schedule/create")]
        public ActionResult CreateSchedule(int? id)
        {
            RubberRoller rubberRoller = _db.rubberRollers.FirstOrDefault(r => r.id == id);
            if (rubberRoller == null) return RedirectToAction("CreateSearch");
            ViewData["roller"] = rubberRoller;
            LogAction.log(this._controllerName, "GET", "Requested Schedule-CreateSchedule webpage", User.Identity.GetUserId());
            return View("ScheduleCreateEditForm");
        }

        // POST: Create new Schedule record
        [HttpPost]
        [Route("schedule/create")]
        public ActionResult CreateSchedule(Schedule schedule, FormCollection collection)
        {
            schedule.RubberRoller = _db.rubberRollers.FirstOrDefault(r => r.id == schedule.rollerID);
            schedule.tinplateSize = $"{collection["thickness"]}x{collection["width"]}x{collection["length"]}";
            schedule.status = ScheduleStatus.ACTIVE;

            _db.schedules.Add(schedule);

            int result = _db.SaveChanges();

            if (result > 0)
            {
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "New schedule record has been successfully added.";
                ViewData["schedule"] = schedule;
                LogAction.log(this._controllerName, "GET", "Requested Schedule-BeforeChecklist form webpage", User.Identity.GetUserId());
                return View("BeforeChecklist");
            }
            else
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The schedule record has not been successfully added.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        /**
         * POST: Redirect to create new before issue
         * checklist page after submission of new schedule 
         */
        [HttpPost]
        public ActionResult CreateBeforeChecklist(BeforeRollerIssueChecklist beforeRollerIssueChecklist)
        {
            Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == beforeRollerIssueChecklist.scheduleID);
            if (schedule == null)
                return RedirectToAction("CreateSearch");

            beforeRollerIssueChecklist.dateTime = DateTime.Now;
            beforeRollerIssueChecklist.preparedBy = getCurrentUser();
            beforeRollerIssueChecklist.Schedule = schedule;
            _db.beforeRollerIssueChecklists.Add(beforeRollerIssueChecklist);
            int result = _db.SaveChanges();

            if (result > 0)
            {
                // Success - redirect to active operation list page
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "New operation record has been successfully added.";
                return RedirectToAction("ActiveOperation");
            }
            else
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The before issue checklist has not been successfully added.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        /**
         *  Mark an active operation as complete
         *  Displays the after checklist form
         */
        [Route("schedule/{id}/mark_complete")]
        public ActionResult MarkOperationComplete(int? id)
        {
            Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == id);
            if (id == 0 || schedule == null) return Redirect(Request.UrlReferrer.ToString());
            ViewData["schedule"] = schedule;
            LogAction.log(this._controllerName, "GET", "Requested Schedule-AfterChecklistForm form webpage", User.Identity.GetUserId());
            return View("AfterChecklist");
        }

        /**
         * Completes an operation
         * Add new after production checklist and updates
         * rubber roller status, schedules & location details
         */
        public ActionResult CompleteOperation(FormCollection collection)
        {
            try
            {
                // Retrieve schedule record
                var schedID = Int32.Parse(collection["scheduleID"]);
                Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == schedID);
                if (schedule == null) return RedirectToAction("ActiveOperation");

                // Create new after production checklist
                AfterRollerProductionChecklist after = new AfterRollerProductionChecklist();
                after.dateTime = DateTime.Now;
                after.rollerAppearance = collection["rollerAppearance"];
                after.remarks = collection["remarks"];
                after.scheduleID = schedule.scheduleID;
                after.Schedule = schedule;
                after.preparedBy = getCurrentUser();
                after.rollerSendTo = collection["rollerSendTo"];

                // Update schedule details
                after.Schedule.endMileage = after.Schedule.startMileage + Int32.Parse(collection["mileageRun"]);
                after.Schedule.endDateTime = DateTime.Parse(collection["endDT"]);
                after.Schedule.status = ScheduleStatus.COMPLETED;

                // Update roller status
                after.Schedule.RubberRoller.status = RollerStatus.getStatus(RollerStatus.IN_STORE_ROOM);

                // Update roller location
                RollerLocation currentLocation = after.Schedule.RubberRoller.RollerLocations.LastOrDefault();
                if (currentLocation != null)
                    currentLocation.dateTimeOut = DateTime.Now;

                RollerLocation rollerLocation = new RollerLocation();
                rollerLocation.rollerID = after.Schedule.rollerID;
                rollerLocation.location = (collection["rollerSendTo"] == "Roller Room" ? collection["roomLocation"] : "Waiting to be sent for maintenance after operation");
                rollerLocation.operationLine = 0;
                rollerLocation.RubberRoller = after.Schedule.RubberRoller;
                rollerLocation.dateTimeIn = DateTime.Now;

                // Add new records
                _db.rollerLocations.Add(rollerLocation);
                _db.afterRollerProductionChecklists.Add(after);
                int result = _db.SaveChanges();

                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "Operation has been completed!";
                    return RedirectToAction("ActiveOperation");
                }
                else
                {
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = "Oops! Something went wrong. The after production checklist has not been successfully added.";
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The operation were unable to successfully marked as completed.";
                return RedirectToAction("ActiveOperation");
            }
        }

        // View the details of an operation
        [Route("schedule/{id}/view")]
        public ActionResult ViewOperationDetail(int id)
        {
            Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == id);
            if (id == 0 || schedule == null) return Redirect(Request.UrlReferrer.ToString());
            return View(schedule);
        }

        public ActionResult UpdateOperationDetail(int? id)
        {
            return View();
        }

        // Convenience method to get current user
        private ApplicationUser getCurrentUser()
        {
            var uID = User.Identity.GetUserId();
            return _db.Users.FirstOrDefault(u => u.Id == uID);
        }
    }
}