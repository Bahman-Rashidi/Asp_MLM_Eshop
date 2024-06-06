using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Schema;
using PagedList;

namespace MLM_app.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class PeriodOfProfitController : BaseController
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


        public ActionResult Index(string sortOrder, string searchString, int? page)
        {
            IQueryable<PeriodOfProfit> data;

            data = db.PeriodOfProfits.Select(x => x).OrderByDescending(x => x.CraeteDate);

            //}

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var datares = data.ToPagedList(pageNumber, pageSize);
            return View(datares);
        }



        public ActionResult AddPeriod()
        {



            return View();
        }
        [HttpPost]
        public ActionResult AddPeriod(MLM_app.Models.PeriodOfProfit model)
        {
            using (var db = new ApplicationDbContext())
            {
                model.CraeteDate = DateTime.Now;
                db.PeriodOfProfits.Add(model);

                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        public ActionResult SetCurrentPerid(int id)
        {
            using (var db = new ApplicationDbContext())
            {
              
                var perdiod = db.PeriodOfProfits.FirstOrDefault(x => x.ID == id);
                perdiod.IsCurrentPeriod = true;
                db.SaveChanges();

            }

            return RedirectToAction("Index");

        }

        public JsonResult ProcessPeriodProfit(string id)
             {

                 using (var db= new ApplicationDbContext())
                 {
                     var PeriodId = int.Parse(id);
                     var perdiod = db.PeriodOfProfits.FirstOrDefault(x => x.ID == PeriodId);

                     perdiod.IsCurrentPeriod = false;



                     var Users = db.Users.Select(x => x);
                    


                     foreach (var user in Users)
                     {

                         if (user.AllowGetPeriodicalDividend)
                         {
                             if (user.Dividend == null)
                             {
                                 user.Dividend = 0;
                             }
                             user.Dividend = user.Dividend + user.PeriodicalDividend;
                         user.PeriodicalDividend = 0;
                         }
                         user.AllowGetPeriodicalDividend = false;
                       

                     }

                     perdiod.IsCalculated = true;

                     db.SaveChanges();


                 }

                 return Json("ok ", JsonRequestBehavior.AllowGet);
             }

    }
}