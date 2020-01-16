using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    public class ChecklistController : Controller
    {
        private ApplicationDbContext _db;

        public ChecklistController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }

        public ActionResult BeforeChecklistCreate()
        {
            return View("BeforeChecklistCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BeforeChecklistCreate(BeforeRollerIssueChecklist beforeRollerIssueChecklist)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                Response.Write(dt.ToString("0:dd/MM/yyyy"));
                    
                _db.beforeRollerIssueChecklists.Add(beforeRollerIssueChecklist);
                _db.SaveChanges();
                TempData["saveStatus"] = true;
                TempData["saveStatusMsg"] = "New Before Roller Issue has been successfully added!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

      

        public ActionResult AfterChecklistCreate()
        {
            return View("AfterChecklistCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AfterChecklistCreate(AfterRollerProductionChecklist afterChecklistCreate)
        {
            if (ModelState.IsValid)
            {
                _db.afterRollerProductionChecklists.Add(afterChecklistCreate);
                _db.SaveChanges();
                TempData["saveStatus"] = true;
                TempData["saveStatusMsg"] = "New After Production Checklist has been successfully added!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        // GET: Checklist
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CombineChecklistView()
        {
            return View();
        }
    }
}