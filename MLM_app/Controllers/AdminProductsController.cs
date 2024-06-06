using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;
using MLM_app.Models;

namespace MLM_app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Display product list
        // GET: AdminProducts
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.ProductGroups);
            return View(await products.ToListAsync());
        }
        #endregion

        #region Display products with categorization
        [AllowAnonymous]
        public ActionResult ShowByGroup(int? id)
        {
            var products = (from pg in db.Products
                            where pg.ProductGroupID == id && pg.ProductStatus == true
                            select pg);


            return View(products.ToList());
        }
        #endregion

        #region Compare products
        [AllowAnonymous]
        public ActionResult Compare(int P1 = 0, int P2 = 0, int P3 = 0, int P4 = 0)
        {
            List<int> productIds = new List<int>();
            productIds.Add(P1);
            productIds.Add(P2);
            productIds.Add(P3);
            productIds.Add(P4);
            var products = from p in db.Products
                           where productIds.Contains(p.ProductID)
                           select p;
            return View(products.ToList());
        }
        #endregion

        #region Display product details to the user
        // GET: AdminProducts/Details/5
        [AllowAnonymous]
        [Route("product/{id}/{productName}")]
        public async Task<ActionResult> ProductDetails(int? id, string productName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            product.VisitCount++;
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();

            ViewBag.Info = TempData["info"];
            ViewBag.color = TempData["color"];
            return View(product);
        }

        #endregion

        #region Submit a new product
        // GET: AdminProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductGroupID = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle");
            return View();
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID,ProductGroupID,ProductStatus,ProductTitle,ProductDescription,ProductPrice,Off,CommentIsActive,ImageUrl,ProductThumbnailImageUrl,LicenseNumber,AvailableNumber")] Product product, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                string newFilenameUrl = string.Empty;
                string thumbnailUrl = string.Empty;
                if (ImageUrl != null)
                {
                    string filename = Path.GetFileName(ImageUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(filename);

                    newFilenameUrl = "/Images/Uploads/Products/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    ImageUrl.SaveAs(physicalFilename);

                    thumbnailUrl = Utils.CreateThumbnail(physicalFilename);
                    product.ImageUrl = newFilenameUrl;
                    product.ProductThumbnailImageUrl = thumbnailUrl;
                    product.ProductDescription = Server.HtmlDecode(product.ProductDescription);
                    db.Products.Add(product);
                    await db.SaveChangesAsync();
                }

                else
                {
                    ModelState.AddModelError("ImageUrl", "Please upload the image");

                    ViewBag.ProductGroupID = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", product.ProductGroupID);
                    return View(product);
                   
                }

                return RedirectToAction("Index");
            }

            ViewBag.ProductGroupID = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", product.ProductGroupID);
            return View(product);
        }
        #endregion

        #region Action method for uploading an image in CKEditor
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") + Path.GetExtension(upload.FileName).ToLower();
                    var vFolderPath = Server.MapPath("/Images/Uploads/Products/");
                    if (!Directory.Exists(vFolderPath))
                    {
                        Directory.CreateDirectory(vFolderPath);
                    }
                    vFilePath = Path.Combine(vFolderPath, vFileName);
                    upload.SaveAs(vFilePath);
                    vImagePath = Url.Content("/Images/Uploads/Products/" + vFileName);
                    vMessage = "The image was saved successfully";
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + vImagePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }
        #endregion

        #region Edit 
        // GET: AdminProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductGroupID = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", product.ProductGroupID);
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,ProductGroupID,ProductStatus,ProductTitle,ProductDescription,ProductPrice,Off,CommentIsActive,ImageUrl,ProductThumbnailImageUrl,LicenseNumber,AvailableNumber")] Product product, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                string newFilenameUrl = string.Empty;
                string thumbnailUrl = string.Empty;

                if (ImageUrl != null)
                {
                    string filename = Path.GetFileName(ImageUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Images/Uploads/Products/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    ImageUrl.SaveAs(physicalFilename);

                    thumbnailUrl = Utils.CreateThumbnail(physicalFilename);

                    // If the previous image exists, we delete it
                    if (System.IO.File.Exists(Server.MapPath("~/" + product.ImageUrl)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/" + product.ImageUrl));
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/" + product.ProductThumbnailImageUrl)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/" + product.ProductThumbnailImageUrl));
                    }
                    product.ImageUrl = newFilenameUrl;
                    product.ProductThumbnailImageUrl = thumbnailUrl;
                }
                product.ProductDescription = Server.HtmlDecode(product.ProductDescription);


                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupID = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", product.ProductGroupID);
            return View(product);
        }
        #endregion

        #region Delete 
        // GET: AdminProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region like & disLike
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Like(int id)
        {
            Product product = db.Products.Find(id);

            if (product != null)
            {
                product.Like++;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(product.Like);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DisLike(int? id)
        {
            Product product = db.Products.Find(id);

            if (product != null)
            {
                product.DisLike++;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(product.DisLike);
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
        
    }
}
