using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;

namespace MLM_app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Show news list
        // GET: AdminPages
        public async Task<ActionResult> Index()
        {
            var pages = db.Pages.Include(p => p.ApplicationUser).Include(p => p.PageGropus);
            return View(await pages.ToListAsync());
        }
        #endregion

        #region Show site news by grouping
        [AllowAnonymous]
        public ActionResult ShowByGroup(int? id)
        {
            var pages = (from p in db.Pages
                         where p.PageGroupId == id
                         select p);
            return View(pages.ToList());
        }
        #endregion

        #region  Details 
        // GET: AdminPages/Details/5
        [AllowAnonymous]

        [Route("page/{id}/{pageName}")]
        public async Task<ActionResult> PageDetails(int? id, string pageName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = await db.Pages.FindAsync(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }
        #endregion

        #region Submit
        // GET: AdminPages/Create
        public ActionResult Create()
        {
            // Using 'user' instead of 'application user' for displaying the author's name.
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "EnglishName");

           // Where should we read the data from for the first parameter
           //What is the value of the data value field for the second parameter
           //What is the value of the data text field for the third parameter
            ViewBag.PageGroupId = new SelectList(db.PageGroups, "PageGroupId", "PageGroupTitle");
            return View();
        }

        // POST: AdminPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PageId,PageGroupId,ApplicationUserId,PageTitle,PageSummary,PageText,PageIsActive,CommentIsActive,PageDate")] Page page)
        {
            if (ModelState.IsValid)
            {
                // To decode HTML codes for displaying to the user
                page.PageText = Server.HtmlDecode(page.PageText);
                db.Pages.Add(page);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "EnglishName", page.ApplicationUserId);
            ViewBag.PageGroupId = new SelectList(db.PageGroups, "PageGroupId", "PageGroupTitle", page.PageGroupId);
            return View(page);
        }
        #endregion

        #region Edit 
        // GET: AdminPages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = await db.Pages.FindAsync(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "EnglishName", page.ApplicationUserId);
            ViewBag.PageGroupId = new SelectList(db.PageGroups, "PageGroupId", "PageGroupTitle", page.PageGroupId);
            return View(page);
        }

        // POST: AdminPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PageId,PageGroupId,ApplicationUserId,PageTitle,PageSummary,PageText,PageIsActive,CommentIsActive,PageDate")] Page page)
        {
            if (ModelState.IsValid)
            {
                page.PageText = Server.HtmlDecode(page.PageText);

                db.Entry(page).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "EnglishName", page.ApplicationUserId);
            ViewBag.PageGroupId = new SelectList(db.PageGroups, "PageGroupId", "PageGroupTitle", page.PageGroupId);
            return View(page);
        }
        #endregion

        #region Delete 
        // GET: AdminPages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = await db.Pages.FindAsync(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: AdminPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Page page = await db.Pages.FindAsync(id);
            db.Pages.Remove(page);
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
