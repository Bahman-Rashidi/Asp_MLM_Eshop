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
    public class AdminCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region List of comments
        // GET: AdminComments
        public async Task<ActionResult> Index()
        {
            var comments = db.Comments.Include(c => c.Pages).Include(c => c.Parent).Include(c => c.Products);
            return View(await comments.ToListAsync());
        }
        #endregion

        #region Show Comment Details
        // GET: AdminComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        #endregion

        #region Submit answer for comment
        // is being used to respond to comments
        // GET: AdminComments/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            ViewBag.PageId = comment.PageID;
            ViewBag.ProductId = comment.ProductID;
            ViewBag.ParentId = comment.CommentId;
            ViewBag.ParentContent = comment.CommentContent;
            ViewBag.Author = "Admin";
            ViewBag.AuthorEmail = "Admin@Admin.com";
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: AdminComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ParentId,CommentDate,CommentIsActive,CommentContent,ProductId,PageId,Author,AuthorEmail")] Comment comment)
        {

            Comment ParentComment =  db.Comments.Find(comment.ParentId);
            comment.ProductID = ParentComment.ProductID;
            comment.PageID = ParentComment.PageID;
            comment.SliderId = ParentComment.SliderId;



            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(comment);
        }
        #endregion

        #region Edit 
        // GET: AdminComments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageID = new SelectList(db.Pages, "PageId", "ApplicationUserId", comment.PageID);
            ViewBag.ParentId = new SelectList(db.Comments, "CommentId", "CommentContent", comment.ParentId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductTitle", comment.ProductID);
            ViewBag.SliderId = new SelectList(db.Products, "SliderId", "ProductTitle", comment.SliderId);

            return View(comment);
        }

        // POST: AdminComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentId,PageID,ProductID,SliderId,ParentId,CommentDate,CommentIsActive,CommentContent,Author,AuthorEmail")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PageID = new SelectList(db.Pages, "PageId", "ApplicationUserId", comment.PageID);
            ViewBag.ParentId = new SelectList(db.Comments, "CommentId", "CommentContent", comment.ParentId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductTitle", comment.ProductID);
            ViewBag.SliderId = new SelectList(db.Products, "SliderId", "ProductTitle", comment.ProductID);

            return View(comment);
        }
        #endregion

        #region Delete 
        // GET: AdminComments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: AdminComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
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
