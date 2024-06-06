using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using MLM_app.ViewModels;
using PagedList;

namespace MLM_app.Controllers
{
  [Authorize(Roles = "Administrator")]
    public class DispachPeymentController : BaseController
    {
      //  private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
         //public ActionResult Index()
         //{


         //    return View();
         //}

         public ActionResult Index(string sortOrder, string searchString, int? page)
        {


             //  dar nazar gereftan   baze zamani
            ViewBag.InsertDateTimeSortParm = String.IsNullOrEmpty(sortOrder) ? "InsertDateTime" : "";
            using (var db = new ApplicationDbContext())
            {
                IQueryable<DispatchPeymentVM> data;
                // Those who have paid and are not yet addressed.
                data = db.Payments.Where(x => x.IsDispachedDivide == false && x.IsFinalized == true).Select(x=>new DispatchPeymentVM()
                 {
                     PaymentId = x.PaymentId,
                     Amount =x.Amount,
                     ApplicationUserEmail =db.Users.FirstOrDefault(c=>c.Id==x.ApplicationUserId).Email,
                     InsertDateTime = x.InsertDateTime,
                     SaleReferenceId=x.SaleReferenceId,
                     OrderId = x.OrderId,
                     ApplicationUserId = x.ApplicationUserId
                     
                 });

                switch (sortOrder)

                {

                    case "InsertDateTime":
                        data = data.OrderByDescending(s => s.InsertDateTime);
                        break;

                        break;
                    default:
                        data = data.OrderBy(x => x.InsertDateTime);
                        break;

                }


                int pageSize = 15;
                int pageNumber = (page ?? 1);

                return View(data.ToPagedList(pageNumber, pageSize));

            }




            return View();
        }




      public JsonResult ProcessPeyment(PeymentProcessVm model)
      {

          if (ModelState.IsValid)
          {







          }

          return Json("", JsonRequestBehavior.AllowGet);
      }


    }
}