using KJCFRubberRoller.Models;
using Microsoft.AspNet.Identity;
using PagedList;
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
        private string _controllerName = "BeforeRollerIssueChecklist";

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

        public ActionResult Index(int? i)
        {
            LogAction.log(this._controllerName, "GET", "Requested BeforeRollerIssueChecklist-Index webpage", User.Identity.GetUserId());
            List<BeforeRollerIssueChecklist> beforeRollerIssueChecklists = _db.beforeRollerIssueChecklists.ToList();
            return View(beforeRollerIssueChecklists.ToPagedList(i ?? 1, 20));
        }

        public ActionResult BeforeChecklistCreate()
        {
            LogAction.log(this._controllerName, "GET", "Requested BeforeRollerIssueChecklist-Create webpage", User.Identity.GetUserId());
            return View("BeforeChecklistCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BeforeChecklistCreate(BeforeRollerIssueChecklist beforeRollerIssueChecklist)
        {
            try
            {
                _db.beforeRollerIssueChecklists.Add(beforeRollerIssueChecklist);
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "New Before Rubber Roller Issue Checklist has been successfully added!";
                    LogAction.log(this._controllerName, "POST", "Added New Before Rubber Roller Issue Checklist", User.Identity.GetUserId());
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The Before Rubber Roller Issue Checklist has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

      

        public ActionResult AfterChecklistCreate()
        {
            LogAction.log(this._controllerName, "GET", "Requested AfterChecklistCreate-Create webpage", User.Identity.GetUserId());
            return View("AfterChecklistCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AfterChecklistCreate(AfterRollerProductionChecklist afterRollerProductionChecklist)
        {
            try
            {
                _db.afterRollerProductionChecklists.Add(afterRollerProductionChecklist);
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "New After Rubber Roller Production Checklist has been successfully added!";
                    LogAction.log(this._controllerName, "POST", "Added New After Rubber Roller Production Checklist", User.Identity.GetUserId());
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = "Oops! Something went wrong. The After Rubber Roller Issue Production has not been successfully added.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        // GET: Checklist
    
        public ActionResult CombineChecklistView()
        {
            return View();
        }
    }
}