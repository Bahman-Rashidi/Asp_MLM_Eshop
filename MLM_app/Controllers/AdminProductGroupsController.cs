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
using PagedList;

namespace MLM_app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminProductGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Display list of product categories
        // GET: AdminProductGroups
        public async Task<ActionResult> Index()
        {
            var productGroups = db.ProductGroups.Include(p => p.Parent);
            return View(await productGroups.ToListAsync());
        }
        #endregion

        #region Display product categories with pagination for User
        [AllowAnonymous]
        public ActionResult ShowByGroup(int? id, int page = 1)
        {
            var productGroups = (from pg in db.ProductGroups
                                 where pg.ParentId == id
                                 orderby pg.ProductGroupID
                                 select pg).ToPagedList(page, 3);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductGroupsList", productGroups);
            }

            return View(productGroups);
        }
        #endregion

        #region Display details of product category
        // GET: AdminProductGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = await db.ProductGroups.FindAsync(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }
        #endregion

        #region Submit 
        // GET: AdminProductGroups/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle");
            return View();
        }

        // POST: AdminProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Create([Bind(Include = "ProductGroupID,ParentId,ProductGroupTitle,ImageUrl")] ProductGroup productGroup, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    var fileName = Path.GetFileName(ImageUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(fileName);
                    newFilename = "/Images/Uploads/ProductGroups/" + newFilename;
                    var physicalPath = Server.MapPath(newFilename);
                    ImageUrl.SaveAs(physicalPath);
                    productGroup.ImageUrl = newFilename;
                }

                db.ProductGroups.Add(productGroup);

                await db.SaveChangesAsync();

                if (productGroup.ParentId == null)
                {
                    productGroup.NameInSystem = "Product" + productGroup.ProductGroupID;
                }
                else
                {
                    productGroup.NameInSystem = "Product" + productGroup.ParentId;
                }

                db.Entry(productGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", productGroup.ParentId);
            return View(productGroup);
        }
        #endregion

        #region Edit 
        // GET: AdminProductGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = await db.ProductGroups.FindAsync(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", productGroup.ParentId);
            return View(productGroup);
        }

        // POST: AdminProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductGroupID,ParentId,ProductGroupTitle,ImageUrl")] ProductGroup productGroup, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    var fileName = Path.GetFileName(ImageUrl.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty) + Path.GetExtension(fileName);
                    newFilename = "/Images/Uploads/ProductGroups/" + newFilename;
                    var physicalPath = Server.MapPath(newFilename);
                    ImageUrl.SaveAs(physicalPath);
                    productGroup.ImageUrl = newFilename;

                    if (System.IO.File.Exists(Server.MapPath("~/" + productGroup.ImageUrl)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/" + productGroup.ImageUrl));
                    }
                }
                productGroup.NameInSystem = "Product" + productGroup.ParentId;
                db.Entry(productGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.ProductGroups, "ProductGroupID", "ProductGroupTitle", productGroup.ParentId);
            return View(productGroup);
        }
        #endregion

        #region Delete 
        // GET: AdminProductGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = await db.ProductGroups.FindAsync(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        // POST: AdminProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductGroup productGroup = await db.ProductGroups.FindAsync(id);
            db.ProductGroups.Remove(productGroup);
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


        #region Managing the tree 

        [HttpPost]
        public ActionResult GetProductGroupTreeJson()
        {
            var nodesList = new List<JsTreeNode>();

            var rootNode = new JsTreeNode
{
    id = "dir",
    text = "Product categories",
    icon = Url.Content("~/Content/images/tree_icon.png"),
    a_attr = { href = "http://www.bing.com" }
};



            List<ProductGroup> myProductGroups;
            using (var db = new ApplicationDbContext())
            {

                myProductGroups = db.ProductGroups.Select(x => x).ToList();

            }

            if (myProductGroups != null && myProductGroups.Count > 0)
            {
                foreach (var item in myProductGroups)
                {


                    if (item.ParentId == null)
                    {
                        rootNode.children.Add(new JsTreeNode() { id = item.ProductGroupID.ToString(), text = item.ProductGroupTitle, a_attr = { href = "/AdminProducts/ShowByGroup/" + item.ProductGroupID } });
                        var SubMenu = myProductGroups.Where(x => x.ParentId == item.ProductGroupID);
                        if (SubMenu.Count() > 0)
                        {
                            var sde = rootNode.children.FirstOrDefault(x => x.id == item.ProductGroupID.ToString());
                            this.SubMenu(myProductGroups, item.ProductGroupID, ref sde);

                        }


                    }

                }


            }







            nodesList.Add(rootNode);


            return Json(nodesList, JsonRequestBehavior.AllowGet);
        }


        private void SubMenu(List<ProductGroup> MyMenu, int menuId, ref JsTreeNode SubStrin)
        {
            List<ProductGroup> subMneu = new List<ProductGroup>();
            var itemsMenu = MyMenu.Where(x => x.ParentId == menuId);
            foreach (var items in itemsMenu)
            {

                SubStrin.children.Add(new JsTreeNode() { id = items.ProductGroupID.ToString(), text = items.ProductGroupTitle, a_attr = { href = "/AdminProducts/ShowByGroup/" + items.ProductGroupID } });
                var NewSubMenu = MyMenu.Where(x => x.ParentId == items.ProductGroupID);
                if (NewSubMenu.Count() > 0)
                {
                    var sde = SubStrin.children.FirstOrDefault(x => x.id == items.ProductGroupID.ToString());
                    SubMenu(MyMenu, items.ProductGroupID, ref sde);
                }
            }


        }


        [HttpPost]
        public virtual ActionResult DoJsTreeOperation(JsTreeOperationData data)
        {
            switch (data.Operation)
            {
                case JsTreeOperation.CopyNode:
                case JsTreeOperation.CreateNode:
                    #region CreateNode


                    //ProductGroup
                    var newp = new ProductGroup();
                    newp.ParentId = int.Parse(data.ParentId);
                    newp.ProductGroupTitle = data.Text;
                    if (newp.ParentId == null)
                    {
                        newp.NameInSystem = "Product" + newp.ProductGroupID;
                    }
                    else
                    {
                        newp.NameInSystem = "Product" + newp.ParentId;
                    }
                    using (var db = new ApplicationDbContext())
                    {

                        var q = db.ProductGroups.Add(newp);

                        db.SaveChanges();
                    }
                    //todo: save data
                    // Receive and return the record ID from the database after Submit
                    return Json(new { id = newp.ProductGroupID }, JsonRequestBehavior.AllowGet);
                    #endregion
                case JsTreeOperation.DeleteNode:
                    //todo: save data
                    var deletGroup = new List<int>();
                    #region Delete Node
                    var newD = new ProductGroup();
                    var nodeId = int.Parse(data.Id);
                    deletGroup.Add(nodeId);

                    bool IsHaveChilde = false;
                    using (var db = new ApplicationDbContext())
                    {
                        if (db.ProductGroups.Any(x => x.ParentId == nodeId))
                        {
                            IsHaveChilde = true;
                            newD.ParentId = int.Parse(data.ParentId);


                           
                        }
                    }

                    if (IsHaveChilde)
                    {
                        DetectParentItems(nodeId, ref deletGroup);
                    }



                    var deletGroups = deletGroup.OrderByDescending(x=>x);

                    using (var db = new ApplicationDbContext())
                    {
                        foreach (var item in deletGroups)
                        {
                            ProductGroup productGroup = db.ProductGroups.Find(item);
                            db.ProductGroups.Remove(productGroup);
                           
                            
                        }
                        db.SaveChanges();
                    }


                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);

                    #endregion

                case JsTreeOperation.MoveNode:
                    //todo: save data
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);

                case JsTreeOperation.RenameNode:
                    //todo: save data
                    #region Rename Node

                    var PId = int.Parse(data.Id);
                    using (var db = new ApplicationDbContext())
                    {
                        var q = db.ProductGroups.FirstOrDefault(x => x.ProductGroupID == PId);
                        q.ProductGroupTitle = data.Text;
                        db.SaveChanges();
                    }


                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                    #endregion

                default:
                    throw new InvalidOperationException(string.Format("{0} is not supported.", data.Operation));
            }
        }




        private void DetectParentItems(int nodeId, ref List<int> DetectedNodes)
        {
            var newD = new ProductGroup();

            using (var db = new ApplicationDbContext())
            {
                newD = db.ProductGroups.FirstOrDefault(x => x.ParentId == nodeId);
                DetectedNodes.Add(newD.ProductGroupID);

                if (db.ProductGroups.Any(x => x.ParentId == newD.ProductGroupID))
                {
                    DetectParentItems(newD.ProductGroupID, ref DetectedNodes);
                }
            }

        }

        #endregion
    }
}
