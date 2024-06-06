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
    public class AdminPageGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region List of newsgroups
        // GET: AdminPageGroups
        public async Task<ActionResult> Index()
        {
            return View(await db.PageGroups.ToListAsync());
        }
        #endregion

        #region Details newsgroups
        // GET: AdminPageGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageGroup pageGroup = await db.PageGroups.FindAsync(id);
            if (pageGroup == null)
            {
                return HttpNotFound();
            }
            return View(pageGroup);
        }
        #endregion

        #region newsgroups  Submit  
        // GET: AdminPageGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPageGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PageGroupId,PageGroupTitle")] PageGroup pageGroup)
        {
            if (ModelState.IsValid)
            {
                db.PageGroups.Add(pageGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pageGroup);
        }
        #endregion

        #region Newsgroups Edit 
        // GET: AdminPageGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageGroup pageGroup = await db.PageGroups.FindAsync(id);
            if (pageGroup == null)
            {
                return HttpNotFound();
            }
            return View(pageGroup);
        }

        // POST: AdminPageGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PageGroupId,PageGroupTitle")] PageGroup pageGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pageGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pageGroup);
        }
        #endregion

        #region Newsgroups Delete 
        // GET: AdminPageGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageGroup pageGroup = await db.PageGroups.FindAsync(id);
            if (pageGroup == null)
            {
                return HttpNotFound();
            }
            return View(pageGroup);
        }

        // POST: AdminPageGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PageGroup pageGroup = await db.PageGroups.FindAsync(id);
            db.PageGroups.Remove(pageGroup);
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
