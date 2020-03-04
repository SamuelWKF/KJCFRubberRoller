﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using KJCFRubberRoller.Models;
using System.Web.Security;
using System.Collections.Generic;
using PagedList;

namespace KJCFRubberRoller.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private string _controllerName = "Account";

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Home");
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationDbContext _db = new ApplicationDbContext();
                    var user = _db.Users.Where(u => u.staffID == model.StaffId.ToUpper()).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.status != 0)
                        {
                            var result = await SignInManager.PasswordSignInAsync(user.Email, model.Password, false, shouldLockout: false);
                            switch (result)
                            {
                                case SignInStatus.Success:
                                    return RedirectToLocal(returnUrl);
                                case SignInStatus.LockedOut:
                                    return View("Lockout");

                                case SignInStatus.Failure:
                                default:
                                    ModelState.AddModelError("", "ALERT : Please enter the correct staff id and password.");
                                    return View(model);
                            }
                        }

                    }
                    

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "ALERT : Oops! Something wrong. Please try again later.");
                    return View(model);
                }
                
            }
            ModelState.AddModelError("", "ALERT : Invalid login attempt."); return View(model);



            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.StaffId, model.Password, false, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");

            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        private SelectList getUserRoles()
        {
            //set priority for Role
            int[] defaultRoleId = { 2, 1, 3, 4 };
            ApplicationDbContext _db = new ApplicationDbContext();
            // Retrieve user roles
            var roleList = _db.Roles
                    .AsEnumerable()
                    .Select((v, i) => new
                    {
                        ID = defaultRoleId[i],
                        name = v.Name
                    }).ToList();

            return new SelectList(roleList, "ID", "name");
        }

        // GET: /staff/list
        [Route("Staff/List")]
        [Authorize(Roles = "Executive,Manager")]
        public ActionResult List(int? i)
        {
            var currentUserID = User.Identity.GetUserId();
            ApplicationDbContext _db = new ApplicationDbContext();
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == currentUserID);
            List<ApplicationUser> users = null;
            if (user.position == 1)
            {
                users = _db.Users.Where(u=>u.Id!=currentUserID).ToList();
            }
            else
            {
                users = _db.Users.Where(u => u.Id != currentUserID && u.position > user.position).ToList();
            }


            LogAction.log(this._controllerName, "GET", "Requested Account-List webpage", User.Identity.GetUserId());
            return View(users.ToPagedList(i ?? 1, 20));
        }

        //
        // GET: /staff/register
        [Route("Staff/Register")]
        [Authorize(Roles = "Executive,Manager")]
        public ActionResult Register()
        {
            ViewData["userPosition"] = getUserRoles();
            LogAction.log(this._controllerName, "GET", "Requested Account-Register webpage", User.Identity.GetUserId());
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Route("Staff/Register")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Executive,Manager")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            var dbUser = _db.Users.Where(u => u.staffID == model.staffID||u.Email == model.Email).FirstOrDefault();
            if (dbUser != null)
            {
                ViewData["userPosition"] = getUserRoles();
                TempData["formStatus"] = false;
                if (dbUser.staffID == model.staffID) { TempData["formStatusMsg"] = $"<b>ALERT</b>: The staff id is already exist."; }
                else {TempData["formStatusMsg"] = $"<b>ALERT</b>: The email is already exist."; }
                
                return View(model);
            }
            //Only Manager can create/update Manager/Executive account
            int currentUserPosition = UserManager.FindById(User.Identity.GetUserId()).position;
            switch (currentUserPosition)
            {
                case 2:
                    if (model.position <= currentUserPosition)
                    {
                        ViewData["userPosition"] = getUserRoles();
                        TempData["formStatus"] = false;
                        TempData["formStatusMsg"] = $"<b>ALERT</b>: Only Manager can create/update Manager/Executive account";
                        return View(model);
                    }
                    break;
            }
            // Generate random password
            ModelState.Remove("Password");
            model.Password = Membership.GeneratePassword(20, 8);
            LogAction.log(this._controllerName, "POST", $"Password generated for new user. New User: {model.staffID}", User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
             
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    staffID = model.staffID,
                    name = model.name,
                    IC = model.IC,
                    position = model.position,
                    status = AccountStatus.ACTIVE
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    LogAction.log(this._controllerName, "POST", $"User account successfully created. New User: {model.staffID}", User.Identity.GetUserId());

                    await UserManager.AddToRoleAsync(user.Id, UserRole.getRole(user.position));
                    LogAction.log(this._controllerName, "POST", $"User role added for new user: {model.staffID} - {UserRole.getRole(user.position)}", User.Identity.GetUserId());

                    // Sent account creation email
                    SendMail.sendMail(model.Email,
                        "Rubber Roller Management System Account Creation",
                        "Hi ! "+ model.name +"<br/>An account has been created for use of the Rubber Roller Management System with a temporary password. Please login with ur staff ID and <b>change the password immediately</b> after login." +
                        "<br/><br/>Your credentials are as follow:" +
                        "<br/>Staff Id: " + model.staffID +
                        "<br/>Password: " + model.Password);
                    LogAction.log(this._controllerName, "POST", $"Account creation email sent to new user: {model.staffID}", User.Identity.GetUserId());

                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = "<b>STATUS</b>: Staff details has been successfully added!";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                ModelState.AddModelError("Email", result.Errors.Last());
            }

            // If we got this far, something failed, redisplay form
            ViewData["userPosition"] = getUserRoles();
            TempData["formStatus"] = false;
            TempData["formStatusMsg"] = "<b>ALERT</b>: Oops! Staff details has not been successfully added.";
            return View(model);
        }

        // GET
        [HttpGet]
        [Route("staff/edit/{staffID}")]
        [Authorize(Roles = "Executive,Manager")]
        public ActionResult Edit(string staffID)
        {
            // If ID is not supplied redirect back to list
            if (staffID == null)
                return RedirectToAction("List");

            // Retrieve existing staff details from database
            ApplicationDbContext _db = new ApplicationDbContext();
            ApplicationUser staff = _db.Users.SingleOrDefault(c => c.staffID == staffID);

            // Ensure the retrieved value is not null
            if (staff == null || staff.Id == User.Identity.GetUserId())
                return RedirectToAction("List");

            UpdateViewModel returnModel = new UpdateViewModel
            {
                staffID = staff.staffID,
                Email = staff.Email,
                Id = staff.Id,
                name = staff.name,
                IC = staff.IC,
                position = staff.position,
                status = staff.status
            };

            ViewData["userPosition"] = getUserRoles();
            LogAction.log(this._controllerName, "GET", string.Format("Requested Account-Edit {0} webpage", staff.staffID), User.Identity.GetUserId());
            return View("Edit", returnModel);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("staff/edit/{staffID}")]
        [Authorize(Roles = "Executive,Manager")]
        public ActionResult Update(UpdateViewModel user)
        {
            ViewData["userPosition"] = getUserRoles();
            try
            {
                ApplicationDbContext _db = new ApplicationDbContext();
                var staff = (ApplicationUser)UserManager.FindById(user.Id);
                var ExStaff = _db.Users.FirstOrDefault(u => u.staffID == user.staffID);
                var ExStaffEmail = _db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (ExStaff != null)
                {
                    if (user.Id != ExStaff.Id)
                    {
                        TempData["formStatus"] = false;
                        TempData["formStatusMsg"] = $"<b>ALERT</b>: Staff ID/Email has been taken by another staff.";
                        return View("UpdateManagerProfile", user);
                    }

                }
                if (ExStaffEmail != null)
                {
                    if (user.Id != ExStaffEmail.Id)
                    {
                        TempData["formStatus"] = false;
                        TempData["formStatusMsg"] = $"<b>ALERT</b>: Staff ID/Email has been taken by another staff.";
                        return View("UpdateManagerProfile", user);
                    }

                }
                int currentUserPosition = UserManager.FindById(User.Identity.GetUserId()).position;
                switch (currentUserPosition)
                {
                    case 2:
                        if (user.position <= currentUserPosition)
                        {
                            ViewData["userPosition"] = getUserRoles();
                            TempData["formStatus"] = false;
                            TempData["formStatusMsg"] = $"<b>ALERT</b>: Only Manager can create/update Manager/Executive account";
                            return View("Edit",user);
                        }
                        break;
                }

                UserManager.RemoveFromRole(staff.Id, UserRole.getRole(staff.position));
                    staff.Email = user.Email;
                    staff.staffID = user.staffID;
                    staff.name = user.name;
                    staff.IC = user.IC;
                    staff.position = user.position;
                    staff.status = user.status;
                    UserManager.AddToRole(staff.Id, UserRole.getRole(staff.position));
                
                var result = UserManager.Update(staff);

                if (!result.Succeeded)
                {
                    ViewData["userPosition"] = getUserRoles();
                    TempData["formStatus"] = false;
                    TempData["formStatusMsg"] = $"<b>ALERT</b>: {result.Errors}";
                    return View("Edit", user);
                }

                TempData["formStatus"] = true;
                TempData["formStatusMsg"] = $"<b>STATUS</b>: Staff ({staff.staffID}) details has been successfully updated!";
                LogAction.log(this._controllerName, "POST", $"Staff ({staff.staffID}) details updated", User.Identity.GetUserId());

                //rchechbox for reset password is true
                if (user.isReset)
                {
                    string newPassword = Membership.GeneratePassword(20, 8);
                    string code = UserManager.GeneratePasswordResetToken(user.Id);
                    UserManager.ResetPassword(user.Id, code, newPassword);
                    LogAction.log(this._controllerName, "POST", $"Manager reset account {staff.staffID} password", User.Identity.GetUserId());
                    // Sent password reset email
                    SendMail.sendMail(staff.Email,
                        "Rubber Roller Management System Account Password Reset",
                        "Hi! " + staff.name + "<br/>Your account password has been reset," +
                        "<br/> Please use the temporary password below to login to your account and <b>change the password immediately</b> after login." +
                        "<br/><br/>Your credentials are as follow:" +
                        "<br/>staff Id: " + staff.staffID +
                        "<br/>Password: " + newPassword);
                    LogAction.log(this._controllerName, "POST", $"Account password reset email sent to current user: {staff.staffID}", User.Identity.GetUserId());
                    //display reset password notification
                    TempData["formStatus"] = true;
                    TempData["formStatusMsg"] = $"<b>STATUS</b>: Staff ({staff.staffID}) details has been successfully updated!</br>A new password is sent to the Email: {staff.Email}";
                    LogAction.log(this._controllerName, "POST", $"Staff ({staff.staffID}) details updated", User.Identity.GetUserId());
                    
                }
                return RedirectToAction("List");

            }
            catch (Exception ex)
            {
                TempData["formStatus"] = false;
                TempData["formStatusMsg"] = $"<b>ALERT</b>:{ex.Message} Oops! Something went wrong. Please try again later.";
                LogAction.log(this._controllerName, "POST", "Error: " + ex.Message, User.Identity.GetUserId());
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ModelState.AddModelError("Email", "The email supplied does not exist.");
                    return View(model);
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                //ModelState.AddModelError("Email", "The email supplied does not exist.");
                return View(model);
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager {
            get {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}