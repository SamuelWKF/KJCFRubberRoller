using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace KJCFRubberRoller.Controllers
{
    public class AfterChecklistController : Controller
    {
        private ApplicationDbContext _db;

        public AfterChecklistController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }

        // GET: AfterChecklist
        public ActionResult Index()
        {
            return View();
        }
        [Route("afterchecklist/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("afterchecklist/create")]
        public ActionResult Create(AfterRollerProductionChecklist afterRollerProductionChecklist )
        {
            if (ModelState.IsValid)
            {
                _db.afterRollerProductionChecklists.Add(afterRollerProductionChecklist);
                _db.SaveChanges();
                TempData["saveStatus"] = true;
                TempData["saveStatusMsg"] = "New checklist has been successfully added!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}