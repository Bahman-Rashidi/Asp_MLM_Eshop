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

    public class CurrentPerid
    {

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    [Authorize]
    public class KartableController : BaseController
    {
        private ApplicationUserManager _userManager;
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



        ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {

            var UserName = User.Identity.Name;

            var MyUser = UserManager.FindByName(UserName);










            return View(MyUser);



        }


        public JsonResult getCurentPerod()
        {
            CurrentPerid p = new CurrentPerid();
           var q = db.PeriodOfProfits.FirstOrDefault(x => x.IsCurrentPeriod);
            if (q != null)
            {

                p.StartDate = q.StartDate.ToString();
                p.EndDate = q.EndDate.ToString();

            }
            // var q = db.PeriodOfProfits.Where(x => x.IsCurrentPeriod);
            // var q = db.PeriodOfProfits.Select(x=>new {StartDate=x.StartDate,EndDate=x.EndDate});
            //var q = db.PeriodOfProfits.Where(x=>x.IsCurrentPeriod).Select(x => new { StartDate = x.StartDate, EndDate = x.EndDate }).FirstOrDefault();
            //
            //p.StartDate = q.StartDate.ToString();
            //p.EndDate = q.EndDate.ToString();
            return Json(p, JsonRequestBehavior.AllowGet);





        }
    }
}