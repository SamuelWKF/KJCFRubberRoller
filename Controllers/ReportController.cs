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
    public class ReportController : Controller
    {
        private ApplicationDbContext _db;
        private string _controllerName = "Report";

        public ReportController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }

        [HttpGet]
        [Route("stock_card/search")]
        public ActionResult StockCardSearch()
        {
            // Retrieve rollers
            var rubberRollers = _db.rubberRollers
                .AsEnumerable()
                .Select(r => new
                {
                    ID = r.id,
                    rollerID = $"{r.rollerID} - {r.RollerCategory.size} {r.RollerCategory.description}"
                }).ToList();

            ViewData["rollerList"] = new SelectList(rubberRollers, "ID", "rollerID");
            LogAction.log(this._controllerName, "GET", "Requested Report-StockCardSearch webpage", User.Identity.GetUserId());
            return View();
        }

        [HttpGet]
        public ActionResult StockCard(RubberRoller rubberRoller, int? i)
        {
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.id == rubberRoller.id);

            if (rubber == null) return RedirectToAction("StockCardSearch");

            LogAction.log(this._controllerName, "GET", $"Requested stock card for Roller: {rubber.id}", User.Identity.GetUserId());
            ViewData["rollerID"] = rubber.rollerID;
            List<RollerLocation> rollerLocations = rubber.RollerLocations.ToList();
            return View(rollerLocations.ToPagedList(i ?? 1, 40));
        }


        [HttpGet]
        [Route("roller_usage/search")]
        public ActionResult RollerUsageSearch()
        {
            // Retrieve rollers
            var rubberRollers = _db.rubberRollers
                .AsEnumerable()
                .Select(r => new
                {
                    ID = r.id,
                    rollerID = $"{r.rollerID} - {r.RollerCategory.size} {r.RollerCategory.description}"
                }).ToList();

            ViewData["rollerList"] = new SelectList(rubberRollers, "ID", "rollerID");
            LogAction.log(this._controllerName, "GET", "Requested Report-RollerUsageSearch webpage", User.Identity.GetUserId());
            return View();
        }

        [HttpGet]
        public ActionResult RollerUsage(RubberRoller rubberRoller, int? i)
        {
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.id == rubberRoller.id);

            if (rubber == null) return RedirectToAction("RollerUsageSearch");

            LogAction.log(this._controllerName, "GET", $"Requested roller usage report for Roller: {rubber.rollerID}", User.Identity.GetUserId());
            RubberRoller rubberRollers = _db.rubberRollers.FirstOrDefault(r => r.id == rubber.id);
            ViewData["rollerID"] = rubberRollers.rollerID;
            List<Schedule> schedules = rubberRollers.Schedules
                .Where(o => o.status == ScheduleStatus.COMPLETED)
                .OrderBy(o => o.startDateTime)
                .ToList();
            return View(schedules.ToPagedList(i ?? 1, 40));
        }

        [HttpGet]
        [Route("roller_maintenance/search")]
        public ActionResult RollerMaintenanceSearch()
        {
            // Retrieve rollers
            var rubberRollers = _db.rubberRollers
                .AsEnumerable()
                .Select(r => new
                {
                    ID = r.id,
                    rollerID = $"{r.rollerID} - {r.RollerCategory.size} {r.RollerCategory.description}"
                }).ToList();

            ViewData["rollerList"] = new SelectList(rubberRollers, "ID", "rollerID");
            LogAction.log(this._controllerName, "GET", "Requested Report-RollerMaintenanceSearch webpage", User.Identity.GetUserId());
            return View();
        }

        [HttpGet]
        public ActionResult RollerMaintenance(RubberRoller rubberRoller, int? i)
        {
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.id == rubberRoller.id);

            if (rubber == null) return RedirectToAction("RollerMaintenanceSearch");

            LogAction.log(this._controllerName, "GET", $"Requested roller maintenance history report for Roller: {rubber.rollerID}", User.Identity.GetUserId());
            RubberRoller rubberRollers = _db.rubberRollers.FirstOrDefault(r => r.id == rubber.id);
            ViewData["rollerID"] = rubberRollers.rollerID;
            List<Maintenance> maintenances = rubberRollers.Maintenances
                .Where(m => m.rollerID == rubber.id)
                .Where(m => m.status == KJCFRubberRoller.Models.RollerMaintenance.APPROVED || m.status == KJCFRubberRoller.Models.RollerMaintenance.COMPLETED)
                .ToList();
            return View(maintenances.ToPagedList(i ?? 1, 40));
        }

        [HttpGet]
        [Route("roller_information")]
        public ActionResult RollerInformation(int? i)
        {
            List<RubberRoller> rubber = _db.rubberRollers.ToList();
            LogAction.log(this._controllerName, "GET", $"Requested roller information report", User.Identity.GetUserId());
            return View(rubber.ToPagedList(i ?? 1, 40));
        }
    }
}