using KJCFRubberRoller.Models;
using Microsoft.AspNet.Identity;
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
            LogAction.log("Requested RollerCategory-Index webpage", User.Identity.GetUserId());
            List<RollerCategory> rollerCategories = _db.rollerCategories.ToList();
            return View(rollerCategories);
        }

        // GET: Returns create form
        public ActionResult Create()
        {
            LogAction.log("Requested RollerCategory-Create webpage", User.Identity.GetUserId());
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

            LogAction.log(string.Format("Requested RubberRoller-Edit {0} webpage", id), User.Identity.GetUserId());
            return View("CreateEditForm", rollerCategory);
        }

        // POST: Create new rubber roller category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RollerCategory rollerCategory)
        {
            try
            {
                _db.rollerCategories.Add(rollerCategory);
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "New rubber roller category has been successfully added!";
                    LogAction.log("[RollerCategory-Create] = Added new roller category record", User.Identity.GetUserId());
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller category has not been successfully added.";
                LogAction.log("[RollerCategory-Create] = Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        // POST: Update existing rubber roller category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(RollerCategory rollerCategory)
        {
            try
            {
                // Retrieve existing specific roller category from database
                RollerCategory rollerCat = _db.rollerCategories.SingleOrDefault(c => c.rollerCategoryID == rollerCategory.rollerCategoryID);

                if (rollerCat == null)
                    return RedirectToAction("Index");

                rollerCat.size = rollerCategory.size;
                rollerCat.description = rollerCategory.description;
                rollerCat.minAmount = rollerCategory.minAmount;
                rollerCat.remark = rollerCategory.remark;
                rollerCat.criticalStatus = rollerCategory.criticalStatus;

                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "Rubber roller category has been successfully updated!";
                    LogAction.log("[RollerCategory-Create] = Roller category details updated", User.Identity.GetUserId());
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller category has not been successfully updated.";
                LogAction.log("[RollerCategory-Update] = Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}