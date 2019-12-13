using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    public class RubberRollerController : Controller
    {
        private ApplicationDbContext _db;

        // Constructor
        public RollerCategoryController()
        {
            _db = new ApplicationDbContext();
        }

        // Add database dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }

        // GET: RubberRoller
        public ActionResult Index()
        {
            return View();
        }
    }
}