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
using MLM_app.Models;

namespace MLM_app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminSlidersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Display list of sliders
        // GET: AdminSliders
        public async Task<ActionResult> Index()
        {
            return View(await db.Sliders.ToListAsync());
        }
        #endregion

        #region Details
        // GET: AdminSliders/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = await db.Sliders.FindAsync(id);

            ViewBag.Description = slider.Title;
            ViewBag.Titlea = slider.Title;
            ViewBag.Keywords = slider.Title;
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }
        #endregion

        #region Submit New Slider 
        // GET: AdminSliders/Create
        public ActionResult Create()
        {
            // background: #96e0ad;
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "EnglishName");

            return View();
        }

        // POST: AdminSliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SliderId,ImageUrl,Title,Link,Order,ApplicationUserId,SilderSummary,SilderIsActive,PageText")] Slider slider, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {

                    var fileName = Path.GetFileName(ImageUrl.FileName);

                    var strFileExtension = Path.GetExtension(fileName).ToUpper();

                    if (((strFileExtension != ".JPG") &&
                         (strFileExtension != ".JPEG") &&
                         (strFileExtension != ".JPE")) &&
                        (strFileExtension != ".GIF") &&
                        (strFileExtension != ".PNG"))
                    {
                        Danger("Allowed extensions for uploading images are jpeg, png, and gif!");
                        return View();
                    }

                    string strContentType = ImageUrl.ContentType.ToUpper();

                    if ((strContentType != "IMAGE/JPEG") && // Firefox!
                        (strContentType != "IMAGE/PJPEG") && // Internet Explorer!
                        (strContentType != "IMAGE/GIF") &&
                        (strContentType != "IMAGE/PNG"))
                    {
                        Danger("The file extension has been changed!");
                        return View();
                    }

                    if (ImageUrl.ContentLength == 0)
                    {
                        Danger("Currently, file upload is not available!");
                        return View();
                    }

                    if (ImageUrl.ContentLength > 200*1024)
                    {
                        //Danger("Maximum allowed upload size is 200 kilobytes.");
                        //return View();
                    }


                    System.Drawing.Image oImage = System.Drawing.Image.FromStream(ImageUrl.InputStream);

                    if ((oImage.Width != 500) || (oImage.Height != 300))
                    {
                        //Danger("Please note that images must be exactly 500 by 300 pixels (height=500 and width=300)!" );
                        //return View();

                    }
                    var strRootRelativePath = "~/Images/Uploads/Sliders/";

                    if (System.IO.File.Exists(Server.MapPath(strRootRelativePath) + Path.GetFileName(ImageUrl.FileName)))
                    {
                        //return "An image with this name already exists in the image storage path!";
                        //System.IO.File.Delete(Server.MapPath("~/" + productGroup.ImageUrl));
                    }

                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(fileName);
                    newFilename = "/Images/Uploads/Sliders/" + newFilename;
                    var physicalPath = Server.MapPath(newFilename);
                    ImageUrl.SaveAs(physicalPath);


                    slider.ImageUrl = newFilename;
                    db.Sliders.Add(slider);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    Danger("Please select an image.");
                    return View();
                }

             
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "EnglishName");
            return View(slider);
        }
        #endregion

        #region Edit 
        // GET: AdminSliders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = await db.Sliders.FindAsync(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: AdminSliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SliderId,ImageUrl,Title,Link,Order,ApplicationUserId,SilderSummary,SilderIsActive,PageText")] Slider slider, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {

                    var fileName = Path.GetFileName(ImageUrl.FileName);

                    var strFileExtension = Path.GetExtension(fileName).ToUpper();

                    if (((strFileExtension != ".JPG") &&
                         (strFileExtension != ".JPEG") &&
                         (strFileExtension != ".JPE")) &&
                        (strFileExtension != ".GIF") &&
                        (strFileExtension != ".PNG"))
                    {

                        Danger("The allowed extension for uploading images is jpeg, png, and gif!");
                        return View();
                    }

                    string strContentType = ImageUrl.ContentType.ToUpper();

                    if ((strContentType != "IMAGE/JPEG") && // Firefox!
                        (strContentType != "IMAGE/PJPEG") && // Internet Explorer!
                        (strContentType != "IMAGE/GIF") &&
                        (strContentType != "IMAGE/PNG"))
                    {

                        Danger("The file extension has been changed!");
                        return View();
                    }

                    if (ImageUrl.ContentLength == 0)
                    {
                        Danger("File upload encountered an issue. Please try again!");
                        return View();
                    }

                    if (ImageUrl.ContentLength > 200 * 1024)
                    {
                        //return View();
                    }

                    System.Drawing.Image oImage =
                        System.Drawing.Image.FromStream(ImageUrl.InputStream);

                    if ((oImage.Width != 500) || (oImage.Height != 300))
                    {
                        //return View();                
                    }

                    var strRootRelativePath = "~/Images/Uploads/Sliders/";

                    if (System.IO.File.Exists(Server.MapPath(strRootRelativePath) + Path.GetFileName(ImageUrl.FileName)))
                    {
                        //System.IO.File.Delete(Server.MapPath("~/" + productGroup.ImageUrl));
                    }

                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(fileName);

                    newFilename = "/Images/Uploads/Sliders/" + newFilename;
                    var physicalPath = Server.MapPath(newFilename);
                    ImageUrl.SaveAs(physicalPath);

                    string oldmp = Server.MapPath(slider.ImageUrl.ToString());
                    try
                    {
                        // Delete the slider from the server
                        System.IO.File.Delete(oldmp);

                    }
                    catch (Exception ex)
                    {

                    }

                    slider.ImageUrl = newFilename;
                }
                db.Entry(slider).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(slider);
        }
        #endregion

        #region Delete 
        // GET: AdminSliders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = await db.Sliders.FindAsync(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: AdminSliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Slider slider = await db.Sliders.FindAsync(id);
            db.Sliders.Remove(slider);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
