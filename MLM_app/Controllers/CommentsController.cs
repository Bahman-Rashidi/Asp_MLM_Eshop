using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using MLM_app.ViewModels;

namespace MLM_app.Controllers
{
    public class CommentsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Display feedback form and submit feedback
        //[HttpGet]
        //public ActionResult InsertComment(int? pageId, int? productId,int? sliderId )
        //{
        //    ViewBag.pageId = pageId;
        //    ViewBag.productId = productId;
        //    ViewBag.sliderId = sliderId;


        //    return PartialView("_InsertComment");
        //}



        [HttpGet]
        public ActionResult InsertComment(int? pageId, int? productId, int? sliderId, string str, int? ParentId)
        {
            ViewBag.pageId = pageId;
            ViewBag.productId = productId;
            ViewBag.sliderId = sliderId;
            ViewBag.str = str;
            ViewBag.ParentId = ParentId;
            return PartialView("_InsertComment");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult InsertComment(CommentViewModel comment)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        Comment newComment = new Comment();
        //        newComment.CommentDate = DateTime.Now;
        //        newComment.Author = comment.Author;
        //        newComment.AuthorEmail = comment.AuthorEmail;
        //        newComment.CommentContent = comment.CommentContent;

        //        db.Comments.Add(newComment);
        //        db.SaveChanges();
        //        TempData["info"] = "Your message has been successfully sent and will be published after confirmation.";
        //        TempData["color"] = "alert-success";
        //    }



        //    return RedirectToAction("ProductDetails", "AdminProducts", new { id = comment.ProductID });
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertComment(CommentViewModel comment)
        {
            var obj = new Comment();



            if (ModelState.IsValid)
            {

                if (Session["Captcha"] == null || Session["Captcha"].ToString() != comment.Captcha)
                {
                    Danger(" error   ");
                    return RedirectToAction("details", "page", new { id = comment.PageID });
                }

                Comment newComment = new Comment();
                newComment.CommentDate = DateTime.Now;
                newComment.Author = comment.Author;
                newComment.AuthorEmail = comment.AuthorEmail;
                newComment.CommentContent = comment.CommentContent;

                newComment.PageID = comment.PageID;
                newComment.ProductID = comment.ProductID;
                newComment.SliderId = comment.SliderId;
                newComment.ParentId = comment.ParentId;
                int culture = 1;


                var IsPersian = System.Globalization.CultureInfo.CurrentCulture.Name.Contains("fa");
                if (IsPersian)
                {
                    culture = 1;
                }
                else
                {
                    culture = 2;
                }
                newComment.langugaeId = culture;
                obj.CommentId = db.Comments.Add(newComment).CommentId;

                var ggg = db.SaveChanges();
                TempData["info"] = " your message  inserted " ;


                TempData["color"] = "alert-success";
            }

            if (comment.ProductID != null && comment.ProductID != 0)
            {
                // [Route("product/{id}/{productName}")]
                // string link =  HtmlHelper.GenerateLink(this.ControllerContext.RequestContext, System.Web.Routing.RouteTable.Routes, "My link", "SEOrute", "About", "Home", null, null);
                //  string linkd = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/product/" + comment.ProductID + "/"+ comment.str;
                // var url = Url.RouteUrl("SEOrute", new { controller = "product", id = comment.ProductID, productName = "یسیس-sdsd" });
                //http://localhost:20138/product/1/%20%D8%A8%D8%B3%D8%AA%D9%87%20%201%20%DA%A9%DB%8C%D9%84%D9%88%DB%8C%DB%8C
                //  var fff = RedirectToAction("ProductDetails", "product", new { id = obj.CommentId });
                // return Redirect(linkd);
             //   return RedirectToAction("Details", "product", new { id = comment.ProductID });
                return RedirectToAction("ProductDetails", "AdminProducts", new { id = comment.ProductID });
            }
            if (comment.SliderId != null && comment.SliderId != 0)
            {
                return RedirectToAction("Details", "AdminSliders", new { id = comment.SliderId });

            }
            if (comment.PageID != null && comment.PageID != 0)
            {
                Success("your message  added ");
                return RedirectToAction("details", "page", new { id = comment.PageID });
            }

            return RedirectToAction("index", "home");



        }


        #endregion

        #region Display comments
        public ActionResult ShowComment(int? pageId, int? productId, int? sliderId)
        {
            List<Comment> commentsL = new List<Comment>();

            if (pageId != null)
            {
                commentsL = db.Comments.Where(c => c.PageID == pageId && c.CommentIsActive == true).ToList();

            }
            if (productId != null)
            {
                commentsL = db.Comments.Where(c => c.ProductID == productId && c.CommentIsActive == true).ToList();
            }
            if (sliderId != null)
            {
                commentsL = db.Comments.Where(c => c.SliderId == sliderId && c.CommentIsActive == true).ToList();

            }

          //  var comments = db.Comments.Where(c => c.ProductID == productId && c.CommentIsActive == true);
            ViewBag.count = commentsL.Count();
            return PartialView("_ShowComment", commentsL);
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