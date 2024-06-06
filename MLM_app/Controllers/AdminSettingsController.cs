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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Schema;
using Postal;

namespace MLM_app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminSettingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

        #region Index
        // GET: AdminSettings
        public async Task<ActionResult> Index()
        {
            return View(await db.Settings.ToListAsync());
        }
        #endregion

        #region Submit
        // GET: AdminSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SettingId,About,Contact")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Settings.Add(setting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(setting);
        }
        #endregion

        #region Edit
        // GET: AdminSettings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = await db.Settings.FindAsync(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: AdminSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SettingId,About,Contact")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(setting);
        }
        #endregion

        #region Changing the website logo
        [HttpGet]
        public ActionResult Logo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logo(HttpPostedFileBase Picture)
        {

            if (Picture != null)
            {

                var fileName = Path.GetFileName(Picture.FileName);

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

                string strContentType = Picture.ContentType.ToUpper();

                if ((strContentType != "IMAGE/JPEG") && // Firefox!
                    (strContentType != "IMAGE/PJPEG") && // Internet Explorer!
                    (strContentType != "IMAGE/GIF") &&
                    (strContentType != "IMAGE/PNG"))
                {

                    Danger("The file extension has been changed!");
                    return View();
                }

                if (Picture.ContentLength == 0)
                {
                    Danger("Currently, file upload is not available!");
                    return View();
                }

                if (Picture.ContentLength > 150 * 1024)
                {
                    Danger("The maximum allowed upload size is 150 kilobytes!");
                    return View();
                }

                System.Drawing.Image oImage = System.Drawing.Image.FromStream(Picture.InputStream);

                if ((oImage.Width != 200) || (oImage.Height != 200))
                {
                    //Danger("Please note that the images must be 200 by 200 pixels in size (width=200 and height=200)!");
                    //return View();
                }

                var strRootRelativePath = "~/Images/";

                if (System.IO.File.Exists(Server.MapPath(strRootRelativePath) + Path.GetFileName(Picture.FileName)))
                {
                    //Danger("");
                    //return View();                            
                }

                string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(fileName);

                newFilename = "/Images/logo.png";
                var physicalPath = Server.MapPath(newFilename);

                Picture.SaveAs(physicalPath);

                Success("The new logo was successfully submitted");
                return View();
            }
            else
            {
                Danger("You did not select an image!");
            }

            return View();
        }
        #endregion

        #region Send Email

        [HttpGet]
        public ActionResult EmailSend()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EmailSend(string email, string subject, string body, Boolean isSenhvnrimUrl)
        {
            bool isSent;
            if (ModelState.IsValid)
            {

                if (isSenhvnrimUrl)
                {
                    var user = UserManager.FindByEmail(email);
                    string code = UserManager.GenerateEmailConfirmationToken(user.Id);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: System.Web.HttpContext.Current.Request.Url.Scheme);
                    isSent = SendEmail(user.Email, subject, Server.HtmlDecode(body + "\n\n\n" + "Please click on the link below to confirm your name submission"), callbackUrl);
                }
                else
                {
                    isSent = SendEmail(email, subject, Server.HtmlDecode(body), "http://Radamin.com");
                }
                if (isSent)
                {
                    Success("Email has  been Sent");
                }
                else
                {
                    Danger("Email has not been Sent !");
                }

            }
            return View();
        }
        #endregion

        #region  Send Email
        private bool SendEmail(string to, string Subject, string body, string link)
        {
            // Specify the email format within the parentheses
            dynamic email = new Email("TemplateEmail");

            email.To = to;//who recieve email
            email.Title = Subject;//Email subject
            email.body = body;//Email Content
            email.link = link;//Link if exist

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

        #region Managing shopping days

        public ActionResult PurchaseDay()
        {

            var myDays = new List<SpecificPurchaseDay>();
            using (var db = new ApplicationDbContext())
            {
                myDays = db.SpecificPurchaseDays.Select(x => x).ToList();




            }


            return View(myDays);
        }

        #endregion
    }
}
