using KJCFRubberRoller.Controllers.Classes;
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
    [Authorize(Roles = "Executive,Manager,Roller PIC")]
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
            List<Schedule> schedules = _db.schedules.Where(s => s.status != ScheduleStatus.COMPLETED).ToList();
            return View(schedules.ToPagedList(i ?? 1, 40));
        }

        // Displays a list of operation history
        public ActionResult OperationHistory(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested Schedule-OperationHistory webpage", User.Identity.GetUserId());
            List<Schedule> schedules = _db.schedules.Where(s => s.status == ScheduleStatus.COMPLETED).ToList();
            return View(schedules.ToPagedList(i ?? 1, 40));
        }

        // Display search roller form - 1st step of adding a new schedule/operation
        public ActionResult CreateSearch()
        {
            // Retrieve rollers
            var rubberRollers = _db.rubberRollers
                .AsEnumerable()
                .Select(r => new
                {
                    ID = r.id,
                    rollerID = $"{r.rollerID} - {r.RollerCategory.size} {r.RollerCategory.description}",
                    r.status
                }).Where(r => r.status == RollerStatus.getStatus(RollerStatus.IN_STORE_ROOM)).ToList();

            ViewData["rollerList"] = new SelectList(rubberRollers, "ID", "rollerID");
            LogAction.log(this._controllerName, "GET", "Requested Schedule-CreateSearch webpage", User.Identity.GetUserId());
            return View("SearchRoller");
        }

        // Display create schedule record after selecting rubber roller
        [Route("schedule/create")]
        public ActionResult CreateSchedule(int? id)
        {
            RubberRoller rubberRoller = _db.rubberRollers.FirstOrDefault(r => r.id == id);
            if (rubberRoller == null || rubberRoller.status != RollerStatus.getStatus(RollerStatus.IN_STORE_ROOM)) return RedirectToAction("CreateSearch");
            ViewData["roller"] = rubberRoller;
            LogAction.log(this._controllerName, "GET", "Requested Schedule-CreateSchedule webpage", User.Identity.GetUserId());
            return View("ScheduleCreateEditForm");
        }

        // POST: Create new Schedule record
        [HttpPost]
        [Route("schedule/create")]
        public ActionResult CreateSchedule(Schedule schedule, FormCollection collection)
        {
            Schedule existingSched = _db.schedules.FirstOrDefault(s => s.rollerID == schedule.rollerID && s.startDateTime == schedule.startDateTime && s.operationLine == schedule.operationLine);
            if (existingSched != null)
            {
                ViewData["schedule"] = existingSched;
                return View("BeforeChecklist");
            }

            RubberRoller rubberRoller = _db.rubberRollers.FirstOrDefault(r => r.id == schedule.rollerID);
            schedule.RubberRoller = rubberRoller;
            schedule.tinplateSize = $"{collection["thickness"]}x{collection["width"]}x{collection["length"]}";
            schedule.status = ScheduleStatus.PENDING_BEFORE_CHECKLIST;
            schedule.RubberRoller.status = RollerStatus.getStatus(RollerStatus.IN_USE);

            // Update roller opening stock date
            if (rubberRoller.opening_stock_date == null || rubberRoller.isRefurbished)
            {
                rubberRoller.opening_stock_date = DateTime.Now;
            }

            // Reset roller isRefurbish status
            if (rubberRoller.isRefurbished)
            {
                rubberRoller.isRefurbished = false;
            }

            _db.schedules.Add(schedule);

            int result = _db.SaveChanges();

            if (result > 0)
            {
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "<b>STATUS</b>: New schedule record has been successfully added.";
                ViewData["schedule"] = schedule;
                LogAction.log(this._controllerName, "POST", "New schedule details successfully added", User.Identity.GetUserId());
                return View("BeforeChecklist");
            }
            else
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Something went wrong. The schedule record has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Errpr adding wew schedule detail.", User.Identity.GetUserId());
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
            schedule.status = ScheduleStatus.ACTIVE;

            // Update roller location
            bool updateLocatResult = CentralUtilities.UpdateRollerLocation(
                beforeRollerIssueChecklist.Schedule.RubberRoller,
                $"Operation Line {beforeRollerIssueChecklist.Schedule.operationLine}"
                );

            int result = _db.SaveChanges();

            if (result > 0 && updateLocatResult)
            {
                // Success - redirect to active operation list page
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "<b>STATUS</b>: New operation record has been successfully added.";
                LogAction.log(this._controllerName, "POST", "Added new before roller issue checklist", User.Identity.GetUserId());
                return RedirectToAction("ActiveOperation");
            }
            else
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Something went wrong. The before issue checklist has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Error adding new before roller issue checklist", User.Identity.GetUserId());
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
        [HttpPost]
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
                after.Schedule.quantity = Int32.Parse(collection["mileageRun"]);
                after.Schedule.status = ScheduleStatus.COMPLETED;

                // Update roller details
                after.Schedule.RubberRoller.last_usage_date = DateTime.Now;
                after.Schedule.RubberRoller.status = collection["rollerSendTo"] == "Roller Room" ? 
                    RollerStatus.getStatus(RollerStatus.IN_STORE_ROOM) :
                    RollerStatus.getStatus(RollerStatus.IN_STORE_ROOM_ON_HOLD);

                // Update roller location
                bool updateLocatResult = CentralUtilities.UpdateRollerLocation(after.Schedule.RubberRoller,
                    (collection["rollerSendTo"] == "Roller Room" ? collection["roomLocation"] : "Roller is on-hold/Waiting to be sent to maintenance after operation"));

                // Add new records
                _db.afterRollerProductionChecklists.Add(after);
                int result = _db.SaveChanges();

                if (result > 0 && updateLocatResult)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "<b>STATUS</b>: Operation has been completed!";
                    LogAction.log(this._controllerName, "POST", "Added new after roller operation checklist", User.Identity.GetUserId());
                    return RedirectToAction("ActiveOperation");
                }
                else
                {
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Something went wrong. The after production checklist has not been successfully added.";
                    LogAction.log(this._controllerName, "POST", "Error adding new after roller operation checklist", User.Identity.GetUserId());
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Something went wrong. The operation were unable to successfully marked as completed.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                return RedirectToAction("ActiveOperation");
            }
        }

        // View the details of an operation
        [Route("schedule/{id}/view")]
        public ActionResult ViewOperationDetail(int id)
        {
            Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == id);
            if (id == 0 || schedule == null) return Redirect(Request.UrlReferrer.ToString());
            LogAction.log(this._controllerName, "GET", "Requested Schedule-ViewOperationDetail webpage", User.Identity.GetUserId());
            return View(schedule);
        }

        // Display edit page
        [Route("schedule/{id}/edit")]
        public ActionResult EditOperationDetail(int? id)
        {
            if (id == 0)
                return Redirect(Request.UrlReferrer.ToString());
            Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == id);
            if (schedule != null)
            {
                LogAction.log(this._controllerName, "GET", "Requested Schedule-EditOperationDetail webpage", User.Identity.GetUserId());
                return View(schedule);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        // Update the operation detail
        [HttpPost]
        public ActionResult UpdateOperationDetail(FormCollection collection)
        {
            try
            {
                int schedID = Int32.Parse(collection["scheduleID"]);
                Schedule schedule = _db.schedules.FirstOrDefault(s => s.scheduleID == schedID);
                if (schedule == null)
                    return Redirect(Request.UrlReferrer.ToString());

                // Update schedule detail
                schedule.startDateTime = DateTime.Parse(collection["startDateTime"]);
                schedule.operationLine = Int32.Parse(collection["operationLine"]);
                schedule.product = collection["product"];
                schedule.tinplateSize = collection["tinplateSize"];
                schedule.remark = collection["remark"];

                // Update Before checklist
                BeforeRollerIssueChecklist beforeRollerIssueChecklist;
                if (schedule.BeforeRollerIssueChecklists.Count == 0)
                {
                    beforeRollerIssueChecklist = new BeforeRollerIssueChecklist();
                    beforeRollerIssueChecklist.preparedBy = getCurrentUser();
                    beforeRollerIssueChecklist.dateTime = DateTime.Now;
                    beforeRollerIssueChecklist.Schedule = schedule;
                    beforeRollerIssueChecklist.scheduleID = schedule.scheduleID;
                    beforeRollerIssueChecklist.hubsCondition = collection["hubsCondition"];
                    beforeRollerIssueChecklist.nutUsed = collection["nutUsed"];
                    beforeRollerIssueChecklist.rollerRoundness = collection["rollerRoundness"];
                    beforeRollerIssueChecklist.rollerSH = collection["rollerSH"];
                    _db.beforeRollerIssueChecklists.Add(beforeRollerIssueChecklist);
                }
                else
                {
                    beforeRollerIssueChecklist = schedule.BeforeRollerIssueChecklists.First();
                    beforeRollerIssueChecklist.hubsCondition = collection["hubsCondition"];
                    beforeRollerIssueChecklist.nutUsed = collection["nutUsed"];
                    beforeRollerIssueChecklist.rollerRoundness = collection["rollerRoundness"];
                    beforeRollerIssueChecklist.rollerSH = collection["rollerSH"];
                }

                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "<b>STATUS</b>: Operation details has been successfully updated!";
                    LogAction.log(this._controllerName, "POST", "Successfully updated operation detail", User.Identity.GetUserId());
                    return RedirectToAction("ActiveOperation");
                }
                else
                {
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Something went wrong. The operation details has not been successfully updated.";
                    LogAction.log(this._controllerName, "POST", "Error updating operation detail", User.Identity.GetUserId());
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            catch (Exception ex)
            {
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Something went wrong.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        // Convenience method to get current user
        private ApplicationUser getCurrentUser()
        {
            var uID = User.Identity.GetUserId();
            return _db.Users.FirstOrDefault(u => u.Id == uID);
        }
    }
}