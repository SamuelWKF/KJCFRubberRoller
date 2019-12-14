using KJCFRubberRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    public class RollerCategoryController : Controller
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

        // GET: RollerCategory
        public ActionResult Index()
        {
            List<RollerCategory> rollerCategories = _db.rollerCategories.ToList();
            return View(rollerCategories);
        }

        // GET: Returns create form
        public ActionResult Create()
        {
            return View("CreateEditForm");
        }

        // GET: Returns edit form with existing roller category data
        public ActionResult Edit(int? id)
        {
            // Ensure ID is supplied
            if (id == null)
                return RedirectToAction("Index");

            // Retrieve existing specific roller category from database
            RollerCategory rollerCategory = _db.rollerCategories.SingleOrDefault(c => c.rollerCategoryID == id);
            
            // Ensure the retrieved value is not null
            if (rollerCategory == null)
                return RedirectToAction("Index");

            return View("CreateEditForm", rollerCategory);
        }

        // POST: Create new rubber roller category
        [HttpPost]
        public ActionResult Create(RollerCategory rollerCategory)
        {
            if (ModelState.IsValid)
            {
                _db.rollerCategories.Add(rollerCategory);
                _db.SaveChanges();
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "New rubber roller category has been successfully added!";
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }


        // POST: Update existing rubber roller category
        [HttpPost]
        public ActionResult Update(RollerCategory rollerCategory)
        {
            // Retrieve existing specific roller category from database
            RollerCategory rollerCat = _db.rollerCategories.SingleOrDefault(c => c.rollerCategoryID == rollerCategory.rollerCategoryID);

            if (rollerCat == null)
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                rollerCat.size = rollerCategory.size;
                rollerCat.description = rollerCategory.description;
                rollerCat.minAmount = rollerCategory.minAmount;
                rollerCat.remark = rollerCategory.remark;
                rollerCat.criticalStatus = rollerCategory.criticalStatus;

                _db.SaveChanges();
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "Rubber roller category has been successfully updated!";
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}