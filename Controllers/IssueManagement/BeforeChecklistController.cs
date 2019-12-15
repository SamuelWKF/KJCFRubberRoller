using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers.IssueManagement
{
    public class BeforeChecklistController : Controller
    {
        private ApplicationDbContext _db;

        public BeforeChecklistController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }
        // GET: BeforeChecklist
        public ActionResult Index()
        {
            return View();
        }

        [Route("beforechecklist/create")]
        public ActionResult Create()
        {
            return View();
        }
        [Route("beforechecklist/create")]
        // POST: Create new rubber roller category
        [HttpPost]
        public ActionResult Create(BeforeRollerIssueChecklist beforeRollerIssueChecklist)
        {
            if (ModelState.IsValid)
            {
                _db.beforeRollerIssueChecklists.Add(beforeRollerIssueChecklist);
                _db.SaveChanges();
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "New before checklist has been successfully added!";
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }


    }
}