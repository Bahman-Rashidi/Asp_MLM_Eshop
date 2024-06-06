using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using MLM_app.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
namespace MLM_app.Controllers
{
    [Authorize]
    public class UserManageController : Controller
    {


        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
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

        #region User Profile
        [HttpGet]
        public async Task<ActionResult> MyProfile()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(userId);
            bool IsCommingFromShopingCart = false;
            if (HttpContext.Request.UrlReferrer != null)
            {
                var PervisPage = HttpContext.Request.UrlReferrer.ToString();

                if (PervisPage != null && PervisPage != "" && PervisPage.Contains("ShoppingCart"))
                {

                    //is coming from shoping cart
                    applicationUser.IsCommingFromShopingCart = true;
                    IsCommingFromShopingCart = true;
                    ;


                }
            }


       
            applicationUser.PasswordHash = null;
            ViewBag.IsCommingFromShoping = IsCommingFromShopingCart;
            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile([Bind(Include = "Id,kartNumber,EnglishName,EnglishName,Address,NationalCode,DateOfRegistration,ZipCode,DateOfBirth,PhoneNumber,UserName,IsCommingFromShopingCart")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(applicationUser.Id);
                user.IsCommingFromShopingCart = applicationUser.IsCommingFromShopingCart;
                if (user != null)
                {
                    user.UserName = applicationUser.UserName;
                  
                    user.EnglishName = applicationUser.EnglishName;
                    user.DateOfBirth = applicationUser.DateOfBirth;
                    user.NationalCode = applicationUser.NationalCode;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.kartNumber = applicationUser.kartNumber;

                    user.Address = applicationUser.Address;
                    user.ZipCode = applicationUser.ZipCode;
                    IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                    if (!validEmail.Succeeded)
                    {
                        AddErrors(validEmail);
                    }
                    IdentityResult validPass = null;
                    if (applicationUser.PasswordHash != null)
                    {
                        validPass = await UserManager.PasswordValidator.ValidateAsync(applicationUser.PasswordHash);
                        if (validPass.Succeeded)
                        {
                            user.PasswordHash = UserManager.PasswordHasher.HashPassword(applicationUser.PasswordHash);
                        }
                        else
                        {
                            AddErrors(validPass);
                        }
                    }
                    if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded
                            && applicationUser.PasswordHash != null && validPass.Succeeded))
                    {
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return View(applicationUser);
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User  was not found");
                }
            }
            return View(applicationUser);
        }
        #endregion

        #region Customer Orders
        public ActionResult Orders(string sortOrder, string searchString ,int? page)
        {

            //OrderDetailsViewModel
            ViewBag.OrderDate = String.IsNullOrEmpty(sortOrder) ? "OrderDateSortParm" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price" : "";
            string userId = User.Identity.GetUserId();
            var data = db.OrderDetails.Where(o => o.Orders.ApplicationUserId == userId).Select(x => new OrderDetailsViewModel
            {
                OrderDetailID = x.OrderDetailID,
                OrderDate = x.Orders.OrderDate,
                OrderedCount = x.OrderedCount,
                Price = x.Price,
                OrderID = x.OrderID,
                ProductID = x.ProductID,
                ProductTitle = x.Products.ProductTitle,
                Off = x.Off
            });
            switch (sortOrder)
            {
                case "Price":
                    data = data.OrderByDescending(s => s.Price);
                    break;
                case "OrderDateSortParm":
                    data = data.OrderByDescending(s => s.OrderDate);
                    break;         
                    break;
                default:
                    data = data.OrderBy(x=>x.OrderID);
                    break;

            }
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    data = data.Where(s => s..Contains(searchString)
            //                           || s.FirstMidName.Contains(searchString));
            //}

            int pageSize =12;
            int pageNumber = (page ?? 1);
         
            return View(data.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Show errors
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        #region Dispose of the connection
        /// <summary>
        /// Override the Dispose method to dispose 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion


        #region  Sub-user password: Users withou
        public ActionResult InValidUsersNotsubuser(string sortOrder, string searchString, int? page)
        {
            //hadaf en ast ke  shaayd karbari  emailkhod ra
            //confirm karde ama baraye zir majome an hich user
            //name va passi  tolid nashode ?




            var userid = User.Identity.Name;
            // All users whose email has been confirmed and are subordinates of the current User
            var sd = db.Users.Where(x => x.EmailConfirmed == true &&  x.ParentUserId==userid);

            // All users who are not sub-users in the sub-user pass
            var data =
             from c in sd
             where !(from o in db.SubUserPassword
                     select o.ApplicationUserId)
                     
                    .Contains(c.Id)
             select c;
             data=  data.OrderByDescending(x => x.DateOfRegistration);




          int pageSize = 2;
          int pageNumber = (page ?? 1);

          return View(data.ToPagedList(pageNumber, pageSize));



        }



        public ActionResult ChangeConfimEmail(string id)
        {



          var q=  db.Users.FirstOrDefault(x => x.Id == id);

            if (q != null)
            {



                q.EmailConfirmed = false;
                db.SaveChanges();
            }

            return RedirectToAction("InValidUsersNotsubuser");
        }

        #endregion

        #region     Users with invalid email addresses

        public ActionResult InValidUsersInValidEmail(string sortOrder, string searchString, int? page)
        {
            var userid = User.Identity.Name;

            var data = db.Users.Where(x => x.EmailConfirmed == false && x.ParentUserId==userid);
             
            data = data.OrderByDescending(x => x.DateOfRegistration);




            int pageSize = 2;
            int pageNumber = (page ?? 1);

            return View(data.ToPagedList(pageNumber, pageSize));



        }



        public ActionResult ChangeEmailPage(string id)
        {

            var q = db.Users.FirstOrDefault(x => x.Id == id);

            if (q != null)
            {





            }


            return View(q);

        }
        [HttpPost]
        public ActionResult ChangeEmailPage(MLM_app.Models.ApplicationUser model)
        {

            var q = db.Users.FirstOrDefault(x => x.Id == model.Id);

            if (q != null)
            {


                q.Email = model.Email;

                q.UserName= model.Email;
                db.SaveChanges();


            }


        return    RedirectToAction("InValidUsersInValidEmail");

        }

        //public ActionResult ChangeEmail(MLM_app.Models.ApplicationUser model)
        //{
        //    var q = db.Users.FirstOrDefault(x => x.Id == model.Id);

        //    if (q != null)
        //    {


        //        q.Email = model.Email;
        //        db.SaveChanges();


        //    }


        //    RedirectToAction("InValidUsersInValidEmail");
        //}

        #endregion

        #region       Management of users with issues
        //  jostesjoo baraye karbar
        //moshakahs kardan  en ke  subuser pass darad ya na...
        //  aya  email  confimed shode ya na
        //

        public ActionResult InValidUsers(string sortOrder, string searchString, string EmailConfimed, int? page)
        {

            //  har chi karbare  moshkel dar  be tartib  az  paiin be bala  sort beshe  10 ta 10 ta

            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "DateOfRegistration" : "";
            string userId = User.Identity.GetUserId();
            //var data = db.OrderDetails.Where(o => o.Orders.ApplicationUserId == userId);
            //var data = db.OrderDetails.Where(o => o.Orders.ApplicationUserId == userId);
            var data = db.Users.Where(o => o.EmailConfirmed==false);

            var query =
             from c in db.Users
             where !(from o in db.SubUserPassword
                     select o.ApplicationUserId)
                    .Contains(c.Id)
             select c;

            return View();



        }

        #endregion
    }
}