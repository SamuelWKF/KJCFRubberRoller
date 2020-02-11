using KJCFRubberRoller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
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
        private string _controllerName = "RubberRoller";

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
        public ActionResult Index(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested RubberRoller-Index webpage", User.Identity.GetUserId());
            List<RubberRoller> rubberRollers = _db.rubberRollers.ToList();
            return View(rubberRollers.ToPagedList(i ?? 1, 20));
        }

        // GET: RubberRoller
        public ActionResult StockOverview(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested RubberRoller-StockOverview webpage", User.Identity.GetUserId());
            List<RollerCategory> rollerCategories = _db.rollerCategories.ToList();
            int count = 0;
            foreach (var rollerCat in rollerCategories)
            {
                if (rollerCat.RubberRollers.Count < rollerCat.minAmount)
                    count++;
            }
            TempData["totalBelowMinAmount"] = count;
            return View(rollerCategories.ToPagedList(i ?? 1, 20));
        }

        // GET: Returns create form
        public ActionResult Create()
        {
            LogAction.log(this._controllerName, "GET", "Requested RubberRoller-Create webpage", User.Identity.GetUserId());

            // Retrieve roller category
            ViewData["rollerCatList"] = getRollerCategories();
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

            LogAction.log(this._controllerName, "GET", string.Format("Requested RubberRoller-Edit {0} webpage", id), User.Identity.GetUserId());

            // Retrieve roller category
            ViewData["rollerCatList"] = getRollerCategories();
            return View("CreateEditForm", rubberRoller);
        }

        // POST: Create new rubber roller record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RubberRoller rubberRoller, FormCollection collection)
        {
            // Check if roller ID exist from DB
            var dbRoller = _db.rubberRollers.Where(r => r.rollerID == rubberRoller.rollerID).FirstOrDefault();
            if (dbRoller != null)
                ModelState.AddModelError("rollerID", "There is already an existing roller with the same roller ID.");

            if (!ModelState.IsValid || dbRoller != null)
            {
                ViewData["rollerCatList"] = getRollerCategories();
                return View("CreateEditForm", rubberRoller);
            }

            _db.rubberRollers.Add(rubberRoller);

            // Create initial rubber roller location
            RollerLocation rollerLocation = new RollerLocation();
            rollerLocation.dateTimeIn = DateTime.Now;
            rollerLocation.rollerID = rubberRoller.id;
            rollerLocation.RubberRoller = rubberRoller;
            rollerLocation.location = collection["rollLocat"];
            _db.rollerLocations.Add(rollerLocation);

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

        public ActionResult CancoChecklist(int? id)
        {
            LogAction.log(this._controllerName, "GET", "Requested RubberRoller-CancoChecklist webpage", User.Identity.GetUserId());
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
                    LogAction.log(this._controllerName, "POST", "Added new canco record", User.Identity.GetUserId());
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "New rubber roller has been successfully added!";
                }
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
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

                if (rubberRoll == null)
                    return RedirectToAction("Index");

                // Update rubber roller details
                if (rubberRoll.rollerID != rubberRoller.rollerID)
                    rubberRoll.rollerID = rubberRoller.rollerID;
                rubberRoll.rollerCategoryID = rubberRoller.rollerCategoryID;
                rubberRoll.type = rubberRoller.type;
                rubberRoll.usage = rubberRoller.usage;
                rubberRoll.shoreHardness = rubberRoller.shoreHardness;
                rubberRoll.depthOfGroove = rubberRoller.depthOfGroove;
                rubberRoll.diameter = rubberRoller.diameter;
                rubberRoll.condition = rubberRoller.condition;
                rubberRoll.remark = rubberRoller.remark;
                rubberRoll.status = rubberRoller.status;

                int result = _db.SaveChanges();

                //if (rubberRoller.supplier.Equals("Canco") && notCancoRoller)
                //    return RedirectToAction("CancoChecklist", new { rubberRoller.id });

                if (result > 0)
                {
                    LogAction.log(this._controllerName, "POST", "Updated roller record", User.Identity.GetUserId());
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "Rubber roller details has been successfully updated!";
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The rubber roller has not been successfully updated.";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        [HttpGet]
        [Route("rubberroller/stockdetails/{id}")]
        public ActionResult StockDetails(int id, int? i)
        {
            if (id == 0)
                return Redirect(Request.UrlReferrer.ToString());

            LogAction.log(this._controllerName, "GET", $"Requested RubberRoller-StockDetails {id} webpage", User.Identity.GetUserId());
            List<RubberRoller> rubberRollers = _db.rubberRollers.Where(r => r.rollerCategoryID == id).ToList();
            return View(rubberRollers.ToPagedList(i ?? 1, 20));
        }

        [HttpGet]
        [Route("rubberroller/location_history/{id}")]
        public ActionResult LocationHistory(int id, int? i)
        {
            if (id == 0)
                return RedirectToAction("Index");

            LogAction.log(this._controllerName, "GET", $"Requested webpage RubberRoller-LocationHistory for Roller: {id}", User.Identity.GetUserId());
            RubberRoller rubberRollers = _db.rubberRollers.FirstOrDefault(r => r.id == id);
            ViewData["rollerID"] = rubberRollers.rollerID;
            List<RollerLocation> rollerLocations = rubberRollers.RollerLocations.ToList();
            return View(rollerLocations.ToPagedList(i ?? 1, 20));
        }

        [HttpGet]
        [Route("rubberroller/operation_history/{id}")]
        public ActionResult OperationHistory(int id, int? i)
        {
            if (id == 0)
                return RedirectToAction("Index");

            LogAction.log(this._controllerName, "GET", $"Requested webpage RubberRoller-OperationHistory for Roller: {id}", User.Identity.GetUserId());
            RubberRoller rubberRollers = _db.rubberRollers.FirstOrDefault(r => r.id == id);
            ViewData["rollerID"] = rubberRollers.rollerID;
            List<Schedule> schedules = rubberRollers.Schedules
                .Where(o => o.status == ScheduleStatus.COMPLETED)
                .OrderBy(o => o.startDateTime)
                .ToList();
            return View(schedules.ToPagedList(i ?? 1, 20));
        }

        [HttpGet]
        [Route("rubberroller/maintenance_history/{id}")]
        public ActionResult MaintenanceHistory(int id, int? i)
        {
            if (id == 0)
                return RedirectToAction("Index");

            LogAction.log(this._controllerName, "GET", $"Requested webpage RubberRoller-MaintenanceHistory for Roller: {id}", User.Identity.GetUserId());
            RubberRoller rubberRollers = _db.rubberRollers.FirstOrDefault(r => r.id == id);
            ViewData["rollerID"] = rubberRollers.rollerID; 
            List<Maintenance> maintenances = rubberRollers.Maintenances
                .Where(m => m.rollerID == id)
                .Where(m => m.status == RollerMaintenance.APPROVED)
                .ToList();
            return View(maintenances.ToPagedList(i ?? 1, 20));
        }

        [HttpGet]
        [Route("rubberroller/location")]
        public ActionResult Location(int? i)
        {
            LogAction.log(this._controllerName, "GET", $"Requested RubberRoller-Location webpage", User.Identity.GetUserId());
            List<RubberRoller> rubberRollers = _db.rubberRollers.ToList();
            return View(rubberRollers.ToPagedList(i ?? 1, 20));
        }

        private SelectList getRollerCategories()
        {
            // Retrieve roller category
            var rollerCatList = _db.rollerCategories
                .AsEnumerable()
                .Select(s => new
                {
                    ID = s.rollerCategoryID,
                    description = string.Format("{0} - {1}", s.size, s.description)
                }).ToList();

            return new SelectList(rollerCatList, "ID", "description");
        }
    }
}