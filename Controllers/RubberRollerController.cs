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
            List<RubberRoller> rubberRollers = _db.rubberRollers.ToList();
            return View(rubberRollers);
        }

        // GET: RubberRoller
        public ActionResult StockOverview()
        {
            List<RollerCategory> rollerCategories = _db.rollerCategories.ToList();
            return View(rollerCategories);
        }

        // GET: Returns create form
        public ActionResult Create()
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

            return View("CreateEditForm", rubberRoller);
        }

        // POST: Create new rubber roller record
        [HttpPost]
        public ActionResult Create(RubberRoller rubberRoller)
        {
            if (!ModelState.IsValid)
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
                TempData["rollerID"] = rubberRoller.id;
                return RedirectToAction("CancoChecklist");
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

        public ActionResult CancoChecklist()
        {
            if ((int)TempData["rollerID"] == 0)
                return RedirectToAction("Index");

            CancoChecklist cancoChecklist = new CancoChecklist();
            cancoChecklist.RubberRoller = _db.rubberRollers.Find((int)TempData["rollerID"]);
            cancoChecklist.date = DateTime.Now;
            return View("CancoChecklist", cancoChecklist);
        }

        // POST: Create new rubber roller record
        [HttpPost]
        public ActionResult CreateCancoRoller(CancoChecklist cancoChecklist)
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
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "New rubber roller has been successfully added!";
            }
            else
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully added.";
            }
            return RedirectToAction("Create");
        }

        // POST: Update existing rubber roller record
        [HttpPost]
        public ActionResult Update(RubberRoller rubberRoller)
        {
            // Retrieve existing specific rubber roller from database
            RubberRoller rubberRoll = _db.rubberRollers.SingleOrDefault(c => c.id == rubberRoller.id);

            if (rubberRoll == null)
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                rubberRoll.rollerID = rubberRoller.rollerID;
                //rubberRoll.description = rubberRoller.description;
                //rubberRoll.minAmount = rubberRoller.minAmount;
                //rubberRoll.remark = rubberRoller.remark;
                //rubberRoll.criticalStatus = rubberRoller.criticalStatus;

                _db.SaveChanges();
                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = "Rubber roller details has been successfully updated!";
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}