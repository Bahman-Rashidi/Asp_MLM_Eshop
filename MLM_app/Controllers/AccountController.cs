using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using MLM_app.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MLM_app.Models;
using Postal;// send email

namespace MLM_app.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        #region Pawword Generator

        private string GeneratePassword1(string str)
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,";
            allowedChars += ",M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!";
            allowedChars += str;
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            //for (int i = 0; i < Convert.ToInt32(txtPassLength.Text); i++)
            for (int i = 0; i < 6; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }
        private string GeneratePassword2(string str)
        {
            string allowedChars = "";
            allowedChars = ",n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!";
            allowedChars += str;
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            //for (int i = 0; i < Convert.ToInt32(txtPassLength.Text); i++)
            for (int i = 0; i < 6; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }
        private string GeneratePassword3(string str)
        {
            string allowedChars = "";
            allowedChars = "W,5,6,7,8,a,b,c,d,e,f,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,Q,R,S,T,U,V,X,Y,Z,";
            allowedChars += "1,2,3,4,9,0,!";
            allowedChars += str;
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            //for (int i = 0; i < Convert.ToInt32(txtPassLength.Text); i++)
            for (int i = 0; i < 6; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }
        private string GeneratePassword4(string str)
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,1,2,3,4,5,6,7,8,9,0,!p,q,r,s,N,O,P,Q,R,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,";
            //allowedChars += "1,2,3,4,5,6,7,8,9,0,!";
            allowedChars += str;
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            //for (int i = 0; i < Convert.ToInt32(txtPassLength.Text); i++)
            for (int i = 0; i < 6; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }

        #endregion
        #region Login
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            bool IsCommingFromShopingCart = false;
            if (HttpContext.Request.UrlReferrer != null)
            {
                var PervisPage = HttpContext.Request.UrlReferrer.ToString();

                if (PervisPage != null && PervisPage != "" && PervisPage.Contains("ShoppingCart"))
                {

                    //is coming from shoping cart
                    IsCommingFromShopingCart = true;


                }
            }


            // We check that the login page is not displayed again if the user requests it after logging in
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string url = Convert.ToString(Request.UrlReferrer);

                if (!String.IsNullOrEmpty(url))
                {
                    return Redirect(url);
                }
                else
                {
                    return Redirect("http://Radamin.com/");
                }

            }


            ViewBag.ReturnUrl = returnUrl;

            ViewBag.IsCommingFromShoping = IsCommingFromShopingCart;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha)
            {
                Danger(" question is incorrect.");
                return View(model);
            }


            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // We create a token
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    // Specifying the activation page email
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    // Calling Function of  Send Email
                    //Email User 
                    //  Email
                    // Content Email
                    // Activation Link
                    bool isSent = SendEmail(user.Email, "Please click on the link below to confirm your name submission", "Please click on the link below to confirm your name submission", callbackUrl);
                 
                    if (isSent)
                    {
                        ViewBag.info = "Dear User, please activate your account first. Check your email (including spam box) for activating your user environment. Click on the Activation Link";


                        ViewBag.color = "alert-danger";

                    }


                    return View();
                }
            }
            else
            {
                Danger("The username or password is incorrect");
                return View(model);
            }


            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                Danger("Your account has been blocked by the administrator!");
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "The username or password is incorrect");
                    return View(model);
            }
        }
        #endregion

        #region VerifyCode
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
        #endregion

        #region SubUsers

        public ActionResult SybUserChart()
        {

            return View();
        }


        public JsonResult GetOrganChartUsers(string Userid)
        {

            var myUser = new List<UserForChartVm>();
            if (!string.IsNullOrEmpty(Userid))
            {
                using (var db = new ApplicationDbContext())
                {
                    var q = db.Users.Where(x => x.ParentUserId == Userid);

                    if (q != null)
                    {
                        foreach (var items in q)
                        {
                            var rootUser = new UserForChartVm();
                            rootUser.Email = items.Email;
                            rootUser.EnglishName = items.EnglishName;
                            rootUser.ParentUserId = items.ParentUserId;
                            rootUser.UserId = items.Id;
                            rootUser.Location = items.LocationMLM;
                            rootUser.IsPurchased = items.AllowGetPeriodicalDividend;
                            myUser.Add(rootUser);

                        }

                    }



                }
            }

            else
            {
                string currentUserId = User.Identity.GetUserId();
                using (var db = new ApplicationDbContext())
                {
                    var items = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                    var rootUser = new UserForChartVm();
                    rootUser.Email = items.Email;
                    rootUser.EnglishName = items.EnglishName;
                    rootUser.ParentUserId = items.ParentUserId;
                    rootUser.UserId = items.Id;
                    rootUser.Location = items.LocationMLM;
                    rootUser.IsPurchased = items.AllowGetPeriodicalDividend;
                    myUser.Add(rootUser);


                }

            }


            return Json(myUser.OrderBy(x => x.Location), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetSubUserIfno()
        {
            var subUsers = new List<SubalternUsers>();
            //SubUserPass
            string currentUserId = User.Identity.GetUserId();
            #region MyRegion
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                SubUserPassword subUserPasswords =
                    db.SubUserPassword.FirstOrDefault(x => x.ApplicationUserId == currentUserId);
                var r1 = new SubalternUsers { Location = "R1" };

                if (currentUser != null && currentUser.Right1 != null)
                {


                    #region R1

                    var r1u = db.Users.FirstOrDefault(x => x.ParentUserId == currentUserId && x.LocationMLM == "R1");
                    if (r1u != null)
                    {

                        r1.IsExist = true;
                        r1.UserId = currentUser.Right1;
                        r1.UserName = r1u.EnglishName + " " + r1u.Email;
                        if (r1u.LatestDateOfPurchase != null) r1.LastedPurchas = (DateTime)r1u.LatestDateOfPurchase;


                    }


                    #endregion

                }
                if (currentUser.Right1 == null && subUserPasswords !=null)
                {
                    r1.SubUserPass = subUserPasswords.R1_pass;
                }
                subUsers.Add(r1);
                var r2 = new SubalternUsers { Location = "R2" };

                if (currentUser != null && currentUser.Right2 != null)
                {


                    #region R2


                    var r2u = db.Users.FirstOrDefault(x => x.ParentUserId == currentUserId && x.LocationMLM == "R2");
                    if (r2u != null)
                    {

                        r2.IsExist = true;
                        r2.UserId = currentUser.Right1;
                        r2.UserName = r2u.EnglishName + " " + r2u.Email;
                        if (r2u.LatestDateOfPurchase != null) r2.LastedPurchas = (DateTime)r2u.LatestDateOfPurchase;

                    }

                    #endregion
                }
                if (currentUser.Right2 == null && subUserPasswords != null)
                {
                    r2.SubUserPass = subUserPasswords.R2_pass;
                }
                subUsers.Add(r2);
                var l1 = new SubalternUsers { Location = "L1" };

                if (currentUser != null && currentUser.Left1 != null)
                {


                    #region L1
                    var l1u = db.Users.FirstOrDefault(x => x.ParentUserId == currentUserId && x.LocationMLM == "L1");
                    if (l1u != null)
                    {

                        l1.IsExist = true;
                        l1.UserId = currentUser.Right1;
                        l1.UserName = l1u.EnglishName + " " + l1u.Email;
                        if (l1u.LatestDateOfPurchase != null) l1.LastedPurchas = (DateTime)l1u.LatestDateOfPurchase;

                    }


                    #endregion
                }
                if (currentUser.Left1 == null && subUserPasswords != null)
                {
                    l1.SubUserPass = subUserPasswords.L1_pass;
                }
                subUsers.Add(l1);
                var l2 = new SubalternUsers { Location = "L2" };

                if (currentUser != null && currentUser.Left2 != null)
                {


                    #region L2
                    var l2U = db.Users.FirstOrDefault(x => x.ParentUserId == currentUserId && x.LocationMLM == "L2");
                    if (l2U != null)
                    {

                        l2.IsExist = true;
                        l2.UserId = currentUser.Right1;
                        l2.UserName = l2U.EnglishName + " " + l2U.Email;
                        if (l2U.LatestDateOfPurchase != null) l2.LastedPurchas = (DateTime)l2U.LatestDateOfPurchase;

                    }

                    #endregion
                }
                if (currentUser.Left2 == null && subUserPasswords != null)
                {
                    l2.SubUserPass = subUserPasswords.L2_pass;
                }
                subUsers.Add(l2);
            }
            #endregion

            return Json(subUsers, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModelBayPassw model)
        {

            bool ParentExist = false;
            bool PassValid = false;
            string LoacationOfNewUser = string.Empty;
  

            if (ModelState.IsValid)
            {

                //1.  pass valid  parent valid  location not used
                //2. add SubUser  Get IdOf SubUser   
                //3.  Update Parent User SybLocation
                if (Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha)
                {
                    Danger(" question is incorrect.");
                }
                else
                {
                    ApplicationUser parentUser;
                    //  string currentUserId = User.Identity.GetUserId();

                    #region PassValid  ParentExist   LocationSet

                    using (var db = new ApplicationDbContext())
                    {
                        parentUser = db.Users.FirstOrDefault(x => x.NationalCode == model.ParentNationalCode);
                        if (parentUser == null)
                        {
                            Danger("Can not  find  Your parent User  .");
                            return View();
                        }
                        else
                        {
                            ParentExist = true;
                            var Loaction =
                                db.SubUserPassword.FirstOrDefault(
                                    x => x.NationalCode == model.ParentNationalCode);// && (
                            //    x.L1_pass == model.Password ||
                            //    x.L2_pass == model.Password || 
                            //    x.R1_pass == model.Password || 
                            //    x.R2_pass == model.Password));

                            if (Loaction == null || (Loaction.IsL1_passUsed && Loaction.IsL1_passUsed && Loaction.IsL1_passUsed &&
                                Loaction.IsL1_passUsed))
                            {
                                Danger("your  parent  has  already   4 user .");
                                return View();
                            }
                            string LocationMLM = "";

                            if (Loaction.L1_pass == model.ParentPasswordUser && !Loaction.IsL1_passUsed)
                            {
                                PassValid = true;
                                LoacationOfNewUser = "L1";

                            }
                            if (Loaction.L2_pass == model.ParentPasswordUser && !Loaction.IsL2_passUsed)
                            {
                                PassValid = true;
                                LoacationOfNewUser = "L2";

                            }
                            if (Loaction.R1_pass == model.ParentPasswordUser && !Loaction.IsR1_passUsed)
                            {
                                PassValid = true;
                                LoacationOfNewUser = "R1";

                            }
                            if (Loaction.R2_pass == model.ParentPasswordUser && !Loaction.IsR2_passUsed)
                            {
                                PassValid = true;

                                LoacationOfNewUser = "R2";


                            }

                        }

                    }
                    #endregion

                    if (PassValid && ParentExist)
                    {
                        var usera = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, EnglishName = model.EnglishName, ParentUserId = parentUser.Id, LocationMLM = LoacationOfNewUser, NationalCode = model.NationalCode, kartNumber = model.kartNumber, AccountNumber = model.AccountNumber, DateOfRegistration = DateTime.Now ,Address = model.Address};
                        var resultc = await UserManager.CreateAsync(usera, model.Password);




                        #region R2 Generatation
                        if (resultc.Succeeded)
                        {

                            #region set Parent User  Sub User

                            using (var db = new ApplicationDbContext())
                            {
                                var parentUserA = db.Users.FirstOrDefault(x => x.NationalCode == model.ParentNationalCode);
                                var SubUserPass =
                                    db.SubUserPassword.FirstOrDefault(x => x.NationalCode == model.ParentNationalCode);
                                var parentUserEdit = db.Users.FirstOrDefault(x => x.NationalCode == model.ParentNationalCode);
                                switch (LoacationOfNewUser)
                                {
                                    case "L2":
                                        parentUserEdit.Left2 = usera.Id;
                                        SubUserPass.IsL2_passUsed = true;
                                        break;
                                    case "L1":
                                        parentUserEdit.Left1 = usera.Id;
                                        SubUserPass.IsL1_passUsed = true;
                                        break;
                                    case "R2":
                                        parentUserEdit.Right2 = usera.Id;
                                        SubUserPass.IsR2_passUsed = true;
                                        break;
                                    case "R1":
                                        parentUserEdit.Right1 = usera.Id;
                                        SubUserPass.IsR1_passUsed = true;
                                        break;


                                    default:
                                        break;


                                }
                                db.SaveChanges();
                            }
                            #endregion


                            //  Create token
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(usera.Id);

                            // active page  Email
                            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                new { userId = usera.Id, code = code }, protocol: Request.Url.Scheme);
                            // Calling Function of  Send Email
                            //Email User 
                            //  Email tile
                            //  Email content
                            // activation link
                            bool isSent = SendEmail(usera.Email, " name of verification link ",
                                "Please click on this likt  to  confirm your  name ", callbackUrl);
                            if (isSent)
                            {
                                ViewBag.info =
                                  "Please  check  you email ( even spam) , and avtive  toyr  acount ";


                                ViewBag.color = "alert-info";
                            }

                        }
                        else
                        {

                            Danger("this Email alreade Submitted");
                            return View();
                        }
                        #endregion


                    }

                    else
                    {
                        Danger("Password of  your  parent is not correct ");
                        return View();
                    }




                }
                //AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult CheckExistingNationalCode(string NationalCode)
        {

            bool ifNationalCodeExist = false;

            try
            {
                using (var db = new ApplicationDbContext())
                {

                    ifNationalCodeExist = db.Users.Any(x => x.NationalCode == NationalCode);
                }

                // ifEmailExist = NationalCode.Equals("mukeshknayak@gmail.com") ? true : false;

                return Json(!ifNationalCodeExist, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }
        //NationalCode

        [AllowAnonymous]
        public ActionResult CheckExistingEmail(string Email)
        {

            bool ifEmailExist = false;

            try
            {

                using (var db = new ApplicationDbContext())
                {

                    ifEmailExist = db.Users.Any(x => x.Email == Email);
                }

                //ifEmailExist = Email.Equals("mukeshknayak@gmail.com") ? true : false;

                return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult DoesUserNameExist(string userName)
        {

            var user = Membership.GetUser(userName);

            return Json(user == null);
        }

        [HttpPost]
        public JsonResult DoesEmailExist(string email)
        {

            var user = Membership.GetUserNameByEmail(email);

            return Json(user == null);
        }
        #endregion

        #region RegisterSubUser
        // GET: /Account/Register
        // [AllowAnonymous]
        public ActionResult RegisterSubUser()
        {







            return View();
        }



        //
        // POST: /Account/Register
        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterSubUser(RegisterViewModel model)
        {


            if (ModelState.IsValid)
            {
                if (Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha)
                {
                    Danger(" question is incorrect.");
                }
                else
                {

                    string currentUserId = User.Identity.GetUserId();
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EnglishName = model.EnglishName, ParentUserId = currentUserId, LocationMLM = model.LocationMLM, NationalCode = model.NationalCode, PhoneNumber = model.PhoneNumber, kartNumber = model.kartNumber, AccountNumber = model.AccountNumber, DateOfRegistration = DateTime.Now ,Address = model.Address};
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        using (var db = new ApplicationDbContext())
                        {
                            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                            var SubUserPass =
                                db.SubUserPassword.FirstOrDefault(x => x.NationalCode == currentUser.NationalCode);

                            switch (model.LocationMLM)
                            {
                                case "L1":

                                    currentUser.Left1 = user.Id;
                                    user.LocationMLM = "L1";
                                    SubUserPass.IsL1_passUsed = true;
                                    break;
                                case "L2":
                                    currentUser.Left2 = user.Id;
                                    user.LocationMLM = "L2";
                                    SubUserPass.IsL2_passUsed = true;

                                    break;
                                case "R1":
                                    currentUser.Right1 = user.Id;
                                    user.LocationMLM = "R1";
                                    SubUserPass.IsR1_passUsed = true;

                                    break;
                                case "R2":
                                    currentUser.Right2 = user.Id;
                                    user.LocationMLM = "R2";
                                    SubUserPass.IsR2_passUsed = true;

                                    break;
                                default:
                                    break;
                                    ;

                            }

                            db.SaveChanges();
                        }

                        // create  token   
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                        // Calling Function of  Send Email
                        //Email User 
                        // title Email
                        // content Email
                        // Activation Link

                        bool isSent = SendEmail(user.Email, " name of verification link ",
        "Please click on this likt  to  confirm your  name ", callbackUrl);

                        if (isSent)
                        {
                            ViewBag.info = "Please  check  you email ( even spam) , and avtive  toyr  acount ";
                            ViewBag.color = "alert-info";
                        }

                    }
                    else
                    {
                        //foreach (var error in result.Errors)
                        //{
                        //}
                        Danger("this  Email already has been  Submitted ");
                    }




            }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        #endregion


        #region ConfirmEmail
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {



            if (userId == null || code == null)
            {
                return View("Error");
            }

            var NewPasses = new SubUserPassword();
            ApplicationUser user = UserManager.FindById(userId);

            if (user != null && user.EmailConfirmed==false)
            {
                #region Generate Pass  for SubUsers


                using (var db = new ApplicationDbContext())
                {
                    if (!db.SubUserPassword.Any(x => x.ApplicationUserId == user.Id))
                    {
                        try
                        {


                            NewPasses.NationalCode = user.NationalCode;
                            NewPasses.ApplicationUserId = user.Id;
                            NewPasses.L1_pass = GeneratePassword1(",@");
                            NewPasses.L2_pass = GeneratePassword2(",#");
                            NewPasses.R1_pass = GeneratePassword3(",?");
                            NewPasses.R2_pass = GeneratePassword4(",*");

                            NewPasses.CraeteDate = DateTime.Now;
                            db.SubUserPassword.Add(NewPasses);

                            db.SaveChanges();
                        }

                        catch (DbEntityValidationException e)
                        {

                            foreach (var eve in e.EntityValidationErrors)
                            {
                              

                                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(eve.Entry.Entity.GetType().Name + " " + eve.Entry.State));
                                foreach (var ve in eve.ValidationErrors)
                                {
                                  
                                    Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ve.PropertyName + " " + ve.ErrorMessage));
                                }
                            }
                        }




                    }



                }
                #endregion


                #region Detect  ParentUser  Levels


                using (var db = new ApplicationDbContext())
                {
                    #region Parent Users

                    //1 Level Parent
                    var p1 = user.ParentUserId;
                    if (p1 != null)
                    {
                        user.P1 = p1;



                        var q2 = db.Users.FirstOrDefault(x => x.Id == p1 && x.ParentUserId != null);
                        if (q2 != null)
                        {
                            //2  Level Parent

                            var p2 = q2.ParentUserId;
                            user.P2 = p2;
                            var q3 = db.Users.FirstOrDefault(x => x.Id == p2 && x.ParentUserId != null);
                            if (q3 != null)
                            {
                                //3  Level Parent

                                var p3 = q3.ParentUserId;
                                user.P3 = p3;
                                var q4 = db.Users.FirstOrDefault(x => x.Id == p3 && x.ParentUserId != null);
                                if (q4 != null)
                                {
                                    //4  Level Parent

                                    var p4 = q4.ParentUserId;
                                    user.P4 = p4;
                                    var q5 = db.Users.FirstOrDefault(x => x.Id == p4 && x.ParentUserId != null);
                                    if (q5 != null)
                                    {
                                        //5   Level Parent

                                        var p5 = q5.ParentUserId;
                                        user.P5 = p5;
                                        var q6 = db.Users.FirstOrDefault(x => x.Id == p5 && x.ParentUserId != null);
                                        if (q6 != null)
                                        {
                                            //6   Level Parent

                                            var p6 = q6.ParentUserId;
                                            user.P6 = p6;
                                            var q7 = db.Users.FirstOrDefault(x => x.Id == p6 && x.ParentUserId != null);
                                            if (q7 != null)
                                            {
                                                //7   Level Parent

                                                var p7 = q7.ParentUserId;
                                                user.P7 = p7;
                                                var q8 = db.Users.FirstOrDefault(x => x.Id == p7 && x.ParentUserId != null);
                                                if (q8 != null)
                                                {
                                                    //8   Level Parent

                                                    var p8 = q8.ParentUserId;
                                                    user.P8 = p8;
                                                    var q9 = db.Users.FirstOrDefault(x => x.Id == p8 && x.ParentUserId != null);
                                                    if (q9 != null)
                                                    {
                                                        //9   Level Parent

                                                        var p9 = q9.ParentUserId;
                                                        user.P9 = p9;
                                                        var q10 = db.Users.FirstOrDefault(x => x.Id == p9 && x.ParentUserId != null);
                                                        if (q10 != null)
                                                        {
                                                            //10   Level Parent

                                                            var p10 = q9.ParentUserId;
                                                            user.P10 = p10;
                                                        }
                                                    }
                                                }



                                            }
                                        }
                                    }

                                }
                            }

                        }
                    }
                    #endregion

                    UserManager.Update(user);

                }





                #endregion

            }



            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #endregion

        #region ForgotPassword
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
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    Danger("we can not find Name  by usinf this email");

                    return View();
                }

                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {

                    Danger("Your account is not active and there is no password recovery option ");

                    return View();
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                bool IsSent = SendEmail(user.Email, "Reset password", "Please click the link below to reset your password", callbackUrl);

                if (IsSent)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                {
                    return View();
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region ForgotPasswordConfirmation
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ResetPassword
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
                return View("Error");
            else
                return View();
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


            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                //return RedirectToAction("ResetPasswordConfirmation", "Account");

                Danger("No email was found with this nam!");
                return View();
            }


            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }
        #endregion

        #region ResetPasswordConfirmation
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ExternalLogin
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }
        #endregion

        #region SendCode
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
        #endregion

        #region ExternalLoginCallback
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
        #endregion

        #region LogOff
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ExternalLoginFailure
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion

        #region Dispose
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
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
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

        #region  Send Email
        private bool SendEmail(string to, string Subject, string body, string link)
        {
            //specify the  template of  Email
            dynamic email = new Email("TemplateEmail");

            email.To = to;//who recieve email
            email.Title = Subject;//Email subject
            email.body = body;//Email Content
            email.link = link;//Link if exist

            try
            {
                email.Send();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}