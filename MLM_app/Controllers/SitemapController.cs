using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;
using MLM_app.Models;

namespace MLM_app.Controllers
{
    public class SitemapController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Create 
        public XmlSitemapResult Sitemap()
        {
            List<SitemapItem> items = new List<SitemapItem>();

            var products = db.Products.ToList();
            var pages = db.Pages.Where(c=>c.PageIsActive==true).ToList();


            DateTime lastModified = System.DateTime.Now;


            items.Add(new SitemapItem("http://Radamin.com", lastModified, ChangeFrequency.Weekly, 1f));


            items.Add(new SitemapItem("http://Radamin.com/Home/Index", lastModified, ChangeFrequency.Weekly, 1f));

            items.Add(new SitemapItem("http://Radamin.com/Home/Contact", lastModified, ChangeFrequency.Weekly, 1f));


            foreach (var item in products)
            {
                items.Add(new SitemapItem("http://Radamin.com/product/" + item.ProductID + "/" + item.ProductTitle.Replace(" ", "-").ToLower().Replace("c#", "csharp").Replace(".", "-"), item.InsertDateTime.ToUniversalTime(), ChangeFrequency.Monthly, .3f));
            }

            foreach (var item in pages)
            {
                items.Add(new SitemapItem("http://Radamin.com/page/" + item.PageId + "/" + item.PageTitle.Replace(" ", "-").ToLower().Replace("c#", "csharp").Replace(".", "-"), item.PageDate.ToUniversalTime(), ChangeFrequency.Monthly, .3f));
            }

            return new XmlSitemapResult(items);
        }
        #endregion
    }
}