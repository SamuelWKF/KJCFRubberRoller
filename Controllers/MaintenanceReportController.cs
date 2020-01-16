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
    public class MaintenanceReportController : Controller
    {
        private ApplicationDbContext _db;
        private string _controllerName = "MaintenanceReport";

        // Constructor
        public MaintenanceReportController()
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
            LogAction.log(this._controllerName, "GET", "Requested MaintenanceReport-Index webpage", User.Identity.GetUserId());
            List<Maintenance> maintenances  = _db.maintenances.ToList();
            return View(maintenances.ToPagedList(i ?? 1, 20));
        }

        // GET: Returns create form
        public ActionResult Create()
        {
            LogAction.log(this._controllerName, "GET", "Requested MaintenanceReport-Create webpage", User.Identity.GetUserId());
            
            return View("CreateEditMaintenanceRecord");
        }

        // POST: Create new rubber roller Maintenance record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Maintenance maintenance)
        {
            // Check if roller ID exist from DB
            var dbRoller = _db.maintenances.Where(m => m.maintenanceID == maintenance.maintenanceID).FirstOrDefault();
            if (dbRoller != null)
                ModelState.AddModelError("maintenanceID", "There is already an existing report with same ID.");

            if (!ModelState.IsValid || dbRoller != null)
            {
               
                return View("CreateEditMaintenanceRecord", maintenance);
            }

            _db.maintenances.Add(maintenance);
            int result = _db.SaveChanges();

            if (result > 0)
            {
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "New rubber roller has been successfully added!";
            }
            else
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully added.";
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        // POST: Update existing rubber roller record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Maintenance maintenance)
        {
            try
            {
                // Retrieve existing specific rubber roller from database
                Maintenance maintenances = _db.maintenances.SingleOrDefault(m => m.maintenanceID == maintenance.maintenanceID);

                if (maintenances == null)
                    return RedirectToAction("Index");

                // Update rubber roller details
                if (maintenances.rollerID != maintenance.rollerID)
                    maintenances.rollerID = maintenance.rollerID;
                maintenances.diameterCore = maintenance.diameterCore;
                maintenances.diameterRoller = maintenance.diameterRoller;
                maintenances.totalMileage = maintenance.totalMileage;
                maintenances.openingStockDate = maintenance.openingStockDate;
                maintenances.lastProductionLine = maintenance.lastProductionLine;
                maintenances.reason = maintenance.reason;
                maintenances.remark = maintenance.remark;
                maintenances.newDiameter = maintenance.newDiameter;
                maintenances.newShoreHardness = maintenance.newShoreHardness;
                maintenances.correctiveAction = maintenance.correctiveAction;
                maintenances.reportedBy = maintenance.reportedBy;
                maintenances.reportDateTime = maintenance.reportDateTime;

                int result = _db.SaveChanges();

                //if (rubberRoller.supplier.Equals("Canco") && notCancoRoller)
                //    return RedirectToAction("CancoChecklist", new { rubberRoller.id });

                if (result > 0)
                {
                    LogAction.log(this._controllerName, "POST", "Updated roller record", User.Identity.GetUserId());
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "Rubber roller details has been successfully updated!";
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully updated.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


    }
}
