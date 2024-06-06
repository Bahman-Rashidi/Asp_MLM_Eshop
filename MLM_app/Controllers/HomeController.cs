using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;
using MLM_app.Models;
using Postal;

namespace MLM_app.Controllers
{
    public class HomeController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        #region First page
        public ActionResult Index(int page = 1)
        {


           // var db = new ApplicationDbContext();
           // var result = db.Sliders.OrderByDescending(x => x.InsertDateTime);
           // var sd = db.ProductGroups.Where(x => x.ParentId == null).ToList();
            return View();
        }
        #endregion

        #region  
        public ActionResult About()
        {
            var about = db.Settings.FirstOrDefault();

            if (about != null)
                ViewBag.About = about.About;

            return View();
        }
        #endregion

        #region 

        [HttpGet]
        public ActionResult Contact()
        {
            var contact = db.Settings.FirstOrDefault();

            if (contact != null)
                ViewBag.Contact = contact.Contact;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = ("Name,Email,Subject,Body,Captcha"))]Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (Session["Captcha"] == null || Session["Captcha"].ToString() != contact.Captcha)
                {
                    Danger(" question is incorrect.");
                }
                else
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    Success("Your message has been successfully sent");

                }
            }
            var varContact = db.Settings.FirstOrDefault();

            if (varContact != null)
                ViewBag.Contact = varContact.Contact;

            return View(contact);
        }
        #endregion

        #region 
        private bool SendEmail(string to, string mail, string Name, string subject, string message)
        {
            dynamic email = new Email("ContactEmail");

            email.To = to;
            email.email = mail;
            email.Name = Name;
            email.Subject = subject;
            email.Message = message;


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

        #region RSS
        [OutputCache(Duration = 3600, VaryByParam = "none")]
        //[Route("Feed")]
        public virtual ActionResult Rss()
        {
            var items = new List<SyndicationItem>();

            var rssProducts = db.Products.OrderByDescending(c => c.InsertDateTime).ToList();
            foreach (var item in rssProducts)
            {

                string feedTitle = item.ProductTitle;

                var helper = new UrlHelper(this.Request.RequestContext);
                var url = helper.Action("Index", "Home", new { }, Request.IsSecureConnection ? "https" : "http");
                url += "product/" + item.ProductID + "/" + item.ProductTitle;

                var feedPackageItem = new SyndicationItem(feedTitle, item.ProductTitle, new Uri(url));
                feedPackageItem.PublishDate = item.InsertDateTime;
                items.Add(feedPackageItem);
            }

            return new RssResult("Website name", items);
        }
        #endregion



        #region 
        public ActionResult Conditions()
        {
            var about = db.Settings.FirstOrDefault();

            if (about != null)
                ViewBag.About = about.About;

            return View();
        }
        #endregion


        #region 
        [HttpGet]
        public ActionResult Complaint()
        {
            var contact = db.Settings.FirstOrDefault();

            if (contact != null)
                ViewBag.Contact = contact.Contact;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complaint([Bind(Include = ("Name,Email,Subject,Body,Captcha"))]Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (Session["Captcha"] == null || Session["Captcha"].ToString() != contact.Captcha)
                {
                    Danger(" question is incorrect.");
                }
                else
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    Success("Your message has been successfully sent");

                }
            }
            var varContact = db.Settings.FirstOrDefault();

            if (varContact != null)
                ViewBag.Contact = varContact.Contact;

            return View(contact);
        }
        #endregion



    }
}