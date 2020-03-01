using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJCFRubberRoller.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private string _controllerName = "Home";

        public ActionResult Index()
        {
            LogAction.log(this._controllerName, "GET", "Requested Home-Index webpage", User.Identity.GetUserId());
            return View();
        }
    }
}