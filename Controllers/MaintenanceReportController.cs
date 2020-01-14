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
            LogAction.log(this._controllerName, "GET", "Requested RubberRoller-Create webpage", User.Identity.GetUserId());

        
            return View("CreateEditMaintenanceRecord");
        }

        // POST: Create new rubber roller record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RubberRoller rubberRoller)
        {
            // Check if roller ID exist from DB
            var dbRoller = _db.rubberRollers.Where(r => r.rollerID == rubberRoller.rollerID).FirstOrDefault();
            if (dbRoller != null)
                ModelState.AddModelError("rollerID", "There is already an existing roller with the same roller ID.");

            if (!ModelState.IsValid || dbRoller != null)
            {
                
                return View("CreateEditForm", rubberRoller);
            }

            _db.rubberRollers.Add(rubberRoller);
            int result = _db.SaveChanges();

            //if (rubberRoller.supplier.Equals("Canco"))
            //{
            //    return RedirectToAction("CancoChecklist", new { rubberRoller.id });
            //}

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
