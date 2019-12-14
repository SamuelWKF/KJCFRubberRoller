using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    public class BeforeNAfterChecklistController : Controller
    {
        private ApplicationDbContext _db;

        // Constructor
        public BeforeNAfterChecklistController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }
        // GET: BeforeNAfterChecklist
        public ActionResult Index()
        {
            return View();
        }
    }
}