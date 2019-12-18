using KJCFRubberRoller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    public class RubberRollerController : Controller
    {
        private ApplicationDbContext _db;

        // Constructor
        public RubberRollerController()
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
            LogAction.log("Requested RubberRoller-Index webpage", User.Identity.GetUserId());
            List<RubberRoller> rubberRollers = _db.rubberRollers.ToList();
            return View(rubberRollers);
        }

        // GET: RubberRoller
        public ActionResult StockOverview()
        {
            LogAction.log("Requested RubberRoller-StockOverview webpage", User.Identity.GetUserId());
            List<RollerCategory> rollerCategories = _db.rollerCategories.ToList();
            return View(rollerCategories);
        }

        // GET: Returns create form
        public ActionResult Create()
        {
            LogAction.log("Requested RubberRoller-Create webpage", User.Identity.GetUserId());
            // Retrieve roller category
            var rollerCatList = _db.rollerCategories
                .AsEnumerable()
                .Select(s => new
                {
                    ID = s.rollerCategoryID,
                    description = string.Format("{0} - {1}", s.size, s.description)
                }).ToList();

            ViewData["rollerCatList"] = new SelectList(rollerCatList, "ID", "description");
            return View("CreateEditForm");
        }


        // GET: Returns edit form with existing rubber roller data
        public ActionResult Edit(int? id)
        {
            // Ensure ID is supplied
            if (id == null)
                return RedirectToAction("Index");

            // Retrieve existing specific roller category from database
            RubberRoller rubberRoller = _db.rubberRollers.SingleOrDefault(c => c.id == id);

            // Ensure the retrieved value is not null
            if (rubberRoller == null)
                return RedirectToAction("Index");

            // Retrieve roller category
            var rollerCatList = _db.rollerCategories
                .AsEnumerable()
                .Select(s => new
                {
                    ID = s.rollerCategoryID,
                    description = string.Format("{0} - {1}", s.size, s.description)
                }).ToList();

            ViewData["rollerCatList"] = new SelectList(rollerCatList, "ID", "description");

            LogAction.log(string.Format("Requested RubberRoller-Edit {0} webpage", id), User.Identity.GetUserId());
            return View("CreateEditForm", rubberRoller);
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
                // Retrieve roller category
                var rollerCatList = _db.rollerCategories
                    .AsEnumerable()
                    .Select(s => new
                    {
                        ID = s.rollerCategoryID,
                        description = string.Format("{0} - {1}", s.size, s.description)
                    }).ToList();

                ViewData["rollerCatList"] = new SelectList(rollerCatList, "ID", "description");
                return View("CreateEditForm", rubberRoller);
            }

            _db.rubberRollers.Add(rubberRoller);
            int result = _db.SaveChanges();

            if (rubberRoller.supplier.Equals("Canco"))
            {
                return RedirectToAction("CancoChecklist", new { rubberRoller.id});
            }

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

        public ActionResult CancoChecklist(int? id)
        {
            LogAction.log("Requested RubberRoller-CancoChecklist webpage", User.Identity.GetUserId());
            if (id == 0)
                return RedirectToAction("Index");

            CancoChecklist cancoChecklist = new CancoChecklist();
            cancoChecklist.RubberRoller = _db.rubberRollers.Find(id);
            cancoChecklist.date = DateTime.Now;
            return View("CancoChecklist", cancoChecklist);
        }

        // POST: Create new rubber roller record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCancoRoller(CancoChecklist cancoChecklist)
        {
            try
            {
                // Retrieve rubber record & current user ID
                RubberRoller rubber = _db.rubberRollers.Where(r => r.rollerID == cancoChecklist.RubberRoller.rollerID).FirstOrDefault();
                var userId = User.Identity.GetUserId();

                // Create new CancoChecklist record
                CancoChecklist cc = new CancoChecklist();
                cc.rollerID = rubber.id;
                cc.RubberRoller = rubber;
                cc.result = cancoChecklist.result;
                cc.scar_issued = cancoChecklist.scar_issued;
                cc.remarks = cancoChecklist.remarks;
                cc.date = DateTime.Now;
                cc.checkedBy = _db.Users.FirstOrDefault(u => u.Id == userId);

                // Add to database
                _db.cancoChecklists.Add(cc);
                int result = _db.SaveChanges();

                if (result > 0)
                {
                    LogAction.log("[RubberRoller-CreateCancoRoller] = Added new canco record", User.Identity.GetUserId());
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "New rubber roller has been successfully added!";
                }
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully added.";
                LogAction.log("[RubberRoller-CreateCancoRoller] = Error: " + ex.Message, User.Identity.GetUserId());
                return RedirectToAction("Create");
            }
        }

        // POST: Update existing rubber roller record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(RubberRoller rubberRoller)
        {
            try
            {
                // Retrieve existing specific rubber roller from database
                RubberRoller rubberRoll = _db.rubberRollers.SingleOrDefault(c => c.id == rubberRoller.id);
                
                bool notCancoRoller = !rubberRoll.supplier.Equals("Canco");

                if (rubberRoll == null)
                    return RedirectToAction("Index");

                // Update rubber roller details
                if (rubberRoll.rollerID != rubberRoller.rollerID)
                    rubberRoll.rollerID = rubberRoller.rollerID;
                rubberRoll.rollerCategoryID = rubberRoller.rollerCategoryID;
                rubberRoll.type = rubberRoller.type;
                rubberRoll.usage = rubberRoller.usage;
                rubberRoll.supplier = rubberRoller.supplier;
                rubberRoll.diameter = rubberRoller.diameter;
                rubberRoll.condition = rubberRoller.condition;
                rubberRoll.remark = rubberRoller.remark;
                rubberRoll.status = rubberRoller.status;

                int result = _db.SaveChanges();

                if (rubberRoller.supplier.Equals("Canco") && notCancoRoller)
                    return RedirectToAction("CancoChecklist", new { rubberRoller.id });

                if (result > 0)
                {
                    LogAction.log("[RubberRoller-Update] = Updated roller record", User.Identity.GetUserId());
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "Rubber roller details has been successfully updated!";
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                LogAction.log("[RubberRoller-Update] = Error: " + ex.Message, User.Identity.GetUserId());
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully updated.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}