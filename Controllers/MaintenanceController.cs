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
            List<Maintenance> maintenance  = _db.maintenances.ToList();
            return View(maintenance.ToPagedList(i ?? 1, 20));
        }

        // GET: Returns create form
        public ActionResult Create()
        {
            LogAction.log(this._controllerName, "GET", "Requested Maintenance-Create webpage", User.Identity.GetUserId());
            return View("CreateEditMaintenanceRecord");
        }

        //FormCollection will store the submitted form data automatically when the form is submitted
        public ActionResult Create(FormCollection collection)
        {
            Maintenance maintenance = new Maintenance();
            string rollID = collection["rollID"];
            RubberRoller rubber = _db.rubberRollers.FirstOrDefault(r => r.rollerID == rollID);
            return Redirect("CreateEditMaintenanceRecord");
        }

       //POST: Create new rubber roller Maintenance record
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Create(Maintenance maintenance)           
       {
            if (!ModelState.IsValid)
            {
                Maintenance m = new Maintenance();

                var uID = User.Identity.GetUserId();
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == uID);
                maintenance.reportedBy = user;

                m.reportDateTime = DateTime.Now;

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
    }
}
