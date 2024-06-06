using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MLM_app.Controllers
{
    public class UserWithRole
    {
        public string UserName { get; set; } // You can alias the SQL output to give these better names
        public string Name { get; set; }
    }


    public class UserRols
    {

        public UserRols()
        {
            RolIds = new List<string>();

        }

        public string UsreId { get; set; }

        public List<string> RolIds { get; set; }
    }

    [Authorize(Roles = "Administrator")]
    public class AdminUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }


        #region Admin Rols

        public async Task<ActionResult> EditRols(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #endregion
        #region GetUserRols

        [HttpPost]
        public async Task<JsonResult> GetUserRols(string UserId)
        {

            var rolesForUser = UserManager.GetRoles(UserId);
            //  var rols = UserManager.GetRoles(UserId);



            return Json(rolesForUser, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region change Rol


        public JsonResult EditUserRols(UserRols data)
        {
            var context = new ApplicationDbContext();
            //var allUsers = context.Users.ToList();
            var allRoles = context.Roles.ToList();

            foreach (var rol in allRoles)
            {
                UserManager.RemoveFromRole(data.UsreId, rol.Name);
            }

            foreach (var item in data.RolIds)
            {

                if (!string.IsNullOrEmpty(item))
                {
                    var rolName = allRoles.FirstOrDefault(x => x.Id == item).Name;
                    if (!string.IsNullOrEmpty(item) && !UserManager.IsInRole(data.UsreId, rolName))
                    {
                        UserManager.AddToRole(data.UsreId, rolName);
                    }
                }


            }

            return Json("", JsonRequestBehavior.AllowGet);

        }

        //EditUserRols
        #endregion

        #region Show Users
        // GET: Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }
        #endregion

        #region Create User 
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);

                }
            }
            return View(model);
        }
        #endregion

        #region Delete User 
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                //  dar sorati ke karbar hich  farzandi nadashte bashad  mitavan an ra hazf kard
                if (user.Left1 == null && user.Left2 == null && user.Right1 == null && user.Right2 == null)
                {

                    var q = db.SubUserPassword.FirstOrDefault(x => x.ApplicationUserId == user.Id);
                    if (q != null)
                    {
                        // pak kardane subuser pass baraye karbare morede nazar
                        db.SubUserPassword.Remove(q);

                    }
                    // yaftane parent
                    ApplicationUser Parentuser = await UserManager.FindByIdAsync(user.ParentUserId);
                    if (Parentuser != null)
                    {

                        var parentSubUserPass =
                            db.SubUserPassword.FirstOrDefault(x => x.ApplicationUserId == Parentuser.Id);
                        //  refresh  kardane  subuserpass baraye parent

                        #region MyRegion


                        if (Parentuser.Left1 != null && Parentuser.Left1 == user.Id)
                        {
                            Parentuser.Left1 = null;
                            if (parentSubUserPass != null)
                            {
                                parentSubUserPass.IsL1_passUsed = false;
                            }

                        }
                        if (Parentuser.Left2 != null && Parentuser.Left2 == user.Id)
                        {
                            Parentuser.Left2 = null;
                            if (parentSubUserPass != null)
                            {
                                parentSubUserPass.IsL2_passUsed = false;
                            }

                        }
                        if (Parentuser.Right1 != null && Parentuser.Right1 == user.Id)
                        {
                            Parentuser.Right1 = null;
                            if (parentSubUserPass != null)
                            {
                                parentSubUserPass.IsR1_passUsed = false;
                            }

                        }
                        if (Parentuser.Right2 != null && Parentuser.Right2 == user.Id)
                        {
                            Parentuser.Right2 = null;
                            if (parentSubUserPass != null)
                            {
                                parentSubUserPass.IsR2_passUsed = false;
                            }

                        }

                        #endregion
                    }

                    db.SaveChanges();
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Error", result.Errors);
                    }

                }




            }
            else
            {
                return View("Error", new string[] { "You can not delete  the  user " });
            }
            return View("Error", new string[] { "User Not Found" });
        }
        #endregion

        #region Edit User 
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, bool emailConfirmed, string password)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                user.EmailConfirmed = emailConfirmed;
                IdentityResult validEmail
                    = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
            return View(user);
        }
        #endregion

        #region search
        public JsonResult GetUsersList(string criteria)
        {

            var query = (from c in db.Users
                         where c.Email.Contains(criteria)
                         orderby c.Email ascending
                         select new { c.Email }).Distinct();

            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Show errors
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
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


        public JsonResult ShowUserInfo(string email)
        {




            var user = UserManager.FindByEmail(email);
            return Json(user, JsonRequestBehavior.AllowGet);



        }
    }
}