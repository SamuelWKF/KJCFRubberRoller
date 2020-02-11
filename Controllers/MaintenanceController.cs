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
    public class MaintenanceController : Controller
    {
        private ApplicationDbContext _db;
        private string _controllerName = "MaintenanceReport";

        // Constructor
        public MaintenanceController()
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
            LogAction.log(this._controllerName, "GET", "Requested Maintenance-Index webpage", User.Identity.GetUserId());
            List<Maintenance> maintenances = _db.maintenances.ToList();
            return View(maintenances.ToPagedList(i ?? 1, 20));
        }

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
            LogAction.log(this._controllerName, "GET", "Requested Maintenance-CreateSearch webpage", User.Identity.GetUserId());
            return View("CreateSearch");
        }

        // GET: Returns create form
        public ActionResult Create(RubberRoller rubberRoller)
        {
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.id == rubberRoller.id);
            if (rubber == null) return RedirectToAction("CreateSearch");
            ViewData["rubber"] = rubber;
            LogAction.log(this._controllerName, "GET", "Requested Maintenance-Create form webpage", User.Identity.GetUserId());
            return View("CreateEditMaintenance");
        }

        [HttpPost]
        public ActionResult CreateConfirm(FormCollection collection)
        {
            try
            {
                Maintenance maintenance = new Maintenance();
                int rollID = Int32.Parse(collection["rollerID"]);
                RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.id == rollID);
                if (rubber == null)
                    return RedirectToAction("CreateSearch");

                maintenance.title = collection["title"];
                maintenance.rollerID = rubber.id;
                maintenance.RubberRoller = rubber;
                maintenance.sendTo = collection["sendTo"];
                maintenance.diameterRoller = Double.Parse(collection["diameterRoller"]);
                maintenance.diameterCore = Double.Parse(collection["diameterCore"]);
                maintenance.openingStockDate = DateTime.Parse(collection["openingStockDate"]);
                maintenance.lastProductionLine = (collection["lastProdLine"] == "-" ? 0 : Int32.Parse(collection["lastProdLine"].Substring(1)));
                maintenance.totalMileage = (collection["totalMileage"] == "0" ? 0 : Int32.Parse(collection["totalMileage"]));
                maintenance.reason = collection["reason"];
                maintenance.remark = collection["remark"];
                maintenance.newDiameter = Double.Parse(collection["newDiameter"]);
                maintenance.newShoreHardness = collection["newShoreHardness"];
                maintenance.correctiveAction = collection["correctiveAction"];
                maintenance.reportDateTime = DateTime.Now;

                LogAction.log(this._controllerName, "GET", "Redirect Maintenance-CreateConfirm webpage", User.Identity.GetUserId());
                return View(maintenance);
            }
            catch (Exception ex)
            {
                LogAction.log(this._controllerName, "POST", $"Error: {ex.Message}", User.Identity.GetUserId());
                return RedirectToAction("CreateSearch");
            }
        }

        // POST: Create new rubber roller Maintenance record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Maintenance maintenance)
        {
            try
            {
                var uID = User.Identity.GetUserId();
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == uID);
                maintenance.RubberRoller = _db.rubberRollers.FirstOrDefault(r => r.id == maintenance.rollerID);
                maintenance.reportDateTime = DateTime.Now;
                maintenance.reportedBy = user;
                maintenance.RubberRoller.status = RollerStatus.getStatus(4);
                maintenance.status = 1;

                _db.maintenances.Add(maintenance);
                int result = _db.SaveChanges();

                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "Maintenance report has been successfully filed!";
                }
                else
                {
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = "Oops! Something went wrong. Maintenance report has not been successfully filed.";
                }
                LogAction.log(this._controllerName, "POST", $"Created new maintenance record #{maintenance.maintenanceID}", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. Please try again later.";
                LogAction.log(this._controllerName, "POST", $"Error creating new maintenance record: {ex.Message}", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("maintenance/{id}/view")]
        public ActionResult ViewMaintenanceReport(int id)
        {
            LogAction.log(this._controllerName, "GET", "Requested Maintenance-ViewMaintenanceReport webpage", User.Identity.GetUserId());
            if (id == 0)
                return RedirectToAction("Index");

            Maintenance maintenance = _db.maintenances.FirstOrDefault(m => m.maintenanceID == id);
            if (maintenance == null)
                return RedirectToAction("Index");
            return View(maintenance);
        }

        [Authorize(Roles = "Executive,Manager")]
        public ActionResult RejectReport(FormCollection collection)
        {
            try
            {
                var ID = Int32.Parse(collection["maintenanceID"]);
                Maintenance maintenance = _db.maintenances.FirstOrDefault(m => m.maintenanceID == ID);
                if (maintenance == null || maintenance.status == 3)
                    return Redirect(Request.UrlReferrer.ToString());

                var uID = User.Identity.GetUserId();
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == uID);

                maintenance.status = 3;
                maintenance.statusRemark = collection["rejectReason"];
                maintenance.approveDateTime = DateTime.Now;
                maintenance.verfiedBy = user;
                maintenance.RubberRoller.status = RollerStatus.getStatus(2);

                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = $"Maintenance report #{ID} has been rejected successfully.";
                }
                else
                {
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = $"Oops! Something went wrong. Maintenance report #{ID} has not been rejected successfully.";
                }
                LogAction.log(this._controllerName, "POST", $"Rejected maintenance report #{maintenance.maintenanceID}", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = $"Oops! Something went wrong.";
                LogAction.log(this._controllerName, "POST", $"Error rejecting maintenance report: {ex.Message}", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Executive,Manager")]
        public ActionResult ApproveReport(FormCollection collection)
        {
            try
            {
                var ID = Int32.Parse(collection["maintenanceID"]);
                Maintenance maintenance = _db.maintenances.FirstOrDefault(m => m.maintenanceID == ID);
                if (maintenance == null || maintenance.status == 2)
                    return Redirect(Request.UrlReferrer.ToString());

                var uID = User.Identity.GetUserId();
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == uID);

                maintenance.status = 2;
                maintenance.statusRemark = null;
                maintenance.approveDateTime = DateTime.Now;
                maintenance.verfiedBy = user;
                maintenance.RubberRoller.status = RollerStatus.getStatus(5);

                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = $"Maintenance report #{ID} has been successfully approved.";
                }
                else
                {
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = $"Oops! Something went wrong. Maintenance report #{ID} has not been successfully approved.";
                }
                LogAction.log(this._controllerName, "POST", $"Approved maintenance report #{maintenance.maintenanceID}", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = $"Oops! Something went wrong.";
                LogAction.log(this._controllerName, "POST", $"Error approving maintenance report: {ex.Message}", User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
        }

        [Route("maintenance/{id}/edit")]
        public ActionResult Edit(int? id)
        {
            // Ensure ID is supplied
            if (id == null)
                return RedirectToAction("Index");

            // Retrieve existing specific maintenance record from database
            Maintenance maintenance = _db.maintenances.SingleOrDefault(c => c.maintenanceID == id);

            // Ensure the retrieved value is not null
            if (maintenance == null)
                return RedirectToAction("Index");

            if (maintenance.status != 1)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = $"Cannot edit a finalized maintenance report.";
                return RedirectToAction("Index");
            }

            LogAction.log(this._controllerName, "GET", $"Requested Maintenance-Edit {id} webpage", User.Identity.GetUserId());
            return View("CreateEditMaintenance", maintenance);
        }

        // POST: Update existing maintenance record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Maintenance maintenance)
        {
            try
            {
                // Retrieve existing maintenance report from database
                Maintenance main = _db.maintenances.SingleOrDefault(m => m.maintenanceID == maintenance.maintenanceID);

                if (main == null)
                    return RedirectToAction("Index");

                main.diameterCore = maintenance.diameterCore;
                main.openingStockDate = maintenance.openingStockDate;
                main.sendTo = maintenance.sendTo;
                main.reason = maintenance.reason;
                main.remark = maintenance.remark;
                main.newDiameter = maintenance.newDiameter;
                main.newShoreHardness = maintenance.newShoreHardness;
                main.correctiveAction = maintenance.correctiveAction;

                int result = _db.SaveChanges();

                if (result > 0)
                {
                    LogAction.log(this._controllerName, "POST", $"Updated maintenance #{maintenance} record", User.Identity.GetUserId());
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = $"Maintenance Report #{maintenance.maintenanceID} has been successfully updated!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = $"Oops! Something went wrong. The maintenance report #{maintenance.maintenanceID} has not been successfully updated.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}
