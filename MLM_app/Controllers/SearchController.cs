using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;

namespace MLM_app.Controllers
{
    public class SearchController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search

        #region search index method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string strSearch)
        {
            if (String.IsNullOrEmpty(strSearch))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<Product> product =
                db.Products.Where(c => c.ProductTitle.Contains(strSearch) ||  c.ProductGroups.ProductGroupTitle.Contains(strSearch));

            if (!product.Any())
            {
                Warning("Item not found");
            }

            return View(product);
        }
        #endregion
    }
}