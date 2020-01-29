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
        private string _controllerName = "Maintenance";

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

        // GET: Returns create form
        public ActionResult Create()
        {
            LogAction.log(this._controllerName, "GET", "Requested Maintenance-Create webpage", User.Identity.GetUserId());
            return View("CreateEditMaintenanceRecord");
        }

        [HttpGet]
        [Route("maintenance/requestmaintenance/{id}")]
        public ActionResult RequestMaintenance(int id, int? i)
        {
            if (id == 0)
                return RedirectToAction("Index");
            LogAction.log(this._controllerName, "GET", $"Requested webpage Maintenance-RequestForMaintenance for Roller: {id}", User.Identity.GetUserId());
            List<Maintenance> maintenances = _db.maintenances.Where(r => r.maintenanceID == id).ToList();
            return View(maintenances.ToPagedList(i ?? 1, 20));
            
            //return View("RequestMaintenance");
        }

        [HttpGet]
        [Route("maintenance/maintenancehistory/{id}")]
        public ActionResult MaintenanceHistory(int id, int? i)
        {
            if (id == 0)
                return RedirectToAction("Index");
            LogAction.log(this._controllerName, "GET", $"Requested webpage Maintenance-MaintenanceHistory for Roller: {id}", User.Identity.GetUserId());
            List<Maintenance> maintenances = _db.maintenances.Where(r => r.rollerID == id).ToList();
            return View(maintenances.ToPagedList(i ?? 1, 20));
            
            //return View(rollerLocations.ToPagedList(i ?? 1, 20));
        }

        //FormCollection will store the submitted form data automatically when the form is submitted
        public void SavedData(FormCollection collection)
        {
            string rollID = collection["rollID"];
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.rollerID == rollID);
            //return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Returns edit form with existing rubber roller data
        public ActionResult Edit(int? id)
        {
            // Ensure ID is supplied
            if (id == null)
                return RedirectToAction("Index");

            // Retrieve existing specific roller category from database
            Maintenance maintenance = _db.maintenances.SingleOrDefault(c => c.maintenanceID == id);

            // Ensure the retrieved value is not null
            if (maintenance == null)
                return RedirectToAction("Index");

            LogAction.log(this._controllerName, "GET", string.Format("Requested RubberRoller-Edit {0} webpage", id), User.Identity.GetUserId());

            return View("CreateEditMaintenanceRecord", maintenance);
        }

        //POST: Create new rubber roller Maintenance record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Maintenance maintenance)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Get reported person's ID
                    //var uID = User.Identity.GetUserId();
                    //ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == uID);
                    //maintenance.reportedBy = user;
                    //reportDate&time
                    //maintenance.reportDateTime = DateTime.Now;

                    _db.maintenances.Add(maintenance);
                    int result = _db.SaveChanges();
                    if (result > 0)
                    {
                        TempData["formStatus"] = true;
                        TempData["formStatusMsg"] = "New rubber roller category has been successfully added!";
                        LogAction.log(this._controllerName, "POST", "Added new roller category record", User.Identity.GetUserId());
                    }
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller category has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
           /*if (!ModelState.IsValid)
            {
                //Get reported person's ID
                var uID = User.Identity.GetUserId();
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == uID);
                maintenance.reportedBy = user;
                //reportDate&time
                maintenance.reportDateTime = DateTime.Now;

                return RedirectToAction("CreateEditMaintenanceRecord", maintenance);
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
       }*/

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
