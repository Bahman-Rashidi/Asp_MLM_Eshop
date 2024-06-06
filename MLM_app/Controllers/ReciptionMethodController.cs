using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
    public class ReciptionMethodController : Controller //BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }


        #region Show Users
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            return View(await db.ReciptionMethods.Select(x=>x).ToListAsync());

               // var pages = db.ReciptionMethods.Select(x => x);
              //  return view(await pages.tolistasync());
        }


        //public async task<actionresult> index()
        //{
        //    var pages = db.pages.include(p => p.applicationuser).include(p => p.pagegropus);
        //    return view(await pages.tolistasync());
        //}
        #endregion

        #region Create 
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ReciptionMethod model)
        {
            if (ModelState.IsValid)
            {
                db.ReciptionMethods.Add(new ReciptionMethod(){
                    EnReceptionMethodName = model.EnReceptionMethodName,
                    ReceptionMethodText = model.ReceptionMethodText,
                    //FaReceptionMethodName = model.FaReceptionMethodName,
                   Price = model.Price
                });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion



        public ActionResult Details(int id)
        {
            return View(db.ReciptionMethods.FirstOrDefault(x=>x.ReciptionMethodId==id));
        }

        public async Task<ActionResult> DeleteM(int id)
           {

               var page = await db.ReciptionMethods.FindAsync(id);
               db.ReciptionMethods.Remove(page);
               await db.SaveChangesAsync();
               return RedirectToAction("Index");
           }


           #region Edit User 
        public async Task<ActionResult> Edit(int id)
        {
            var q = db.ReciptionMethods.FirstOrDefault(x => x.ReciptionMethodId == id);
            if (q != null)
            {
                return View(q);
            }
            else
            {
                return RedirectToAction("Index", db.ReciptionMethods.Select(x => x).ToList());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ReciptionMethod model)
        {
            var q = db.ReciptionMethods.FirstOrDefault(x => x.ReciptionMethodId == model.ReciptionMethodId);
            if (q != null)
            {
                q.ReceptionMethodText = model.ReceptionMethodText;
                q.EnReceptionMethodName = model.EnReceptionMethodName;
              //  q.FaReceptionMethodName = model.FaReceptionMethodName;
                q.Price = model.Price;
                db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", " Not found");
            }
            return View("Index", db.ReciptionMethods.Select(x => x).ToList());
           // return View("Details", q.ReciptionMethodId);

        }
        #endregion

    }
}