using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using System.Data.Entity;
using MLM_app.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace MLM_app.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Order Details
        // GET: AdminOrders  Order Details
        public ActionResult Index(string sortOrder, string searchString, int? page)
        {
            var data = db.OrderDetails.OrderByDescending(x => x.Orders.OrderDate) ;
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(data.ToPagedList(pageNumber, pageSize));
        }


        // GET: AdminOrders  Factos
        public ActionResult AllOrders(string sortOrder, string searchString, int? page)
        {
            var userId = User.Identity.GetUserId();


            //  dar nazar gereftan   baze zamani
            ViewBag.OrderDate = String.IsNullOrEmpty(sortOrder) ? "OrderDate" : "";
            var userOrders = (from o in db.Orders
                              // where o.ApplicationUserId == userId
                              select new OrdersViewModel()
                              {
                                  OrderID = o.OrderID,
                                  OrderDate = o.OrderDate,
                                  ReciptionMethod = o.ReciptionMethod,
                                  Price = o.ReciptionPrice,
                                  IsFinalized = o.IsFinalized,
                                  //SaleReferenceId=(from oo in db.Orders
                                  //                 join pz in db.Payments on oo.OrderID  equals  pz.OrderId

                                  //                 select pz.SaleOrderId
                                                   
                                  //                 ).where oo.OrderID==pz.OrderId,

                                     SaleReferenceId=db.Payments.FirstOrDefault(x=>x.OrderId==o.OrderID).SaleReferenceId,
                                  ReferenceNumber = db.Payments.FirstOrDefault(x => x.OrderId == o.OrderID).ReferenceNumber,
                                  ApplicationUserId = o.ApplicationUserId,
                                  // In this section, we subtotal to calculate the total amount.
                                  OrderTotal = (from od in db.OrderDetails
                                                join p in db.Products on od.ProductID equals p.ProductID
                                                where od.OrderID == o.OrderID
                                                select (od.Price - (od.Price * od.Off / 100)) * od.OrderedCount).Sum(),
                                  OrderTotalWithReception = (from od in db.OrderDetails
                                                             join p in db.Products on od.ProductID equals p.ProductID
                                                             where od.OrderID == o.OrderID
                                                             select (od.Price - (od.Price * od.Off / 100)) * od.OrderedCount).Sum() + o.ReciptionPrice,

                                  // Order details consist of a list of sub-factors.
                                  OrderDetails = o.OrderDetails
                              }).OrderByDescending(x => x.OrderDate);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (sortOrder == "OrderDate")
            {
                userOrders = userOrders.OrderBy(x => x.OrderDate);
            }
            return View(userOrders.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult GetUserProfile(string userId)
        {

            UserViewModel user = new UserViewModel();
            // var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                ApplicationUser applicationUser = db.Users.FirstOrDefault(x => x.Id == userId);
                if (applicationUser == null)
                {

                    return Json(user, JsonRequestBehavior.AllowGet);
                }
                applicationUser.PasswordHash = null;
                user.Address = applicationUser.Address;
                user.Email = applicationUser.Email;
                user.PhoneNumber = applicationUser.PhoneNumber;
                user.ZipCode = applicationUser.ZipCode;

                if (string.IsNullOrEmpty(user.Address) && string.IsNullOrEmpty(user.PhoneNumber) && string.IsNullOrEmpty(user.ZipCode))
                {
                    user.ValidCOntractInfo = true;
                }
            }


            return Json(user, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Delete order
        [HttpGet]
        public JsonResult DeleteConfirmed(int? id)
        {
            var result = false;
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                db.Orders.Remove(order);
                db.SaveChanges();
            }
            else
            {
                result = true;
            }

            var jsonData = new { result };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Dispose Connection
        /// <summary>
        /// override Dispose method
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


        #region Invoice Details

        public ActionResult details(int id)
        {

            MyOrdersViewModel MyModel = new MyOrdersViewModel();
            List<ShoppingCartViewModel> cartcontents = new List<ShoppingCartViewModel>();


            var order = db.Orders.FirstOrDefault(x => x.OrderID == id);
            if (order != null)
            {
                MyModel.ReciptionMethod = order.ReciptionMethod;

                var orderq = db.ReciptionMethods.FirstOrDefault(x => x.EnReceptionMethodName == order.ReciptionMethod);
                if (orderq != null)
                {
                    MyModel.Price = orderq.Price;

                }



            }

            var q = db.OrderDetails.Where(x => x.OrderID == id);
            if (q != null)
            {
                //   MyModel.ReciptionMethod=q.

                foreach (var item in q)
                {


                    var PId = item.ProductID;
                    // var PTn = item.ProductTableName;

                    var q2 = db.Products.FirstOrDefault(Z => Z.ProductID == PId);
                    cartcontents.Add(new ShoppingCartViewModel()
                    {
                        ProductID = PId,
                        // ProductMeasure = q2.IndexType,
                        ProductPrice = q2.ProductPrice,
                        ProductCount = item.OrderedCount,

                        ProductTitle = q2.ProductTitle,
                        RowTotal = q2.ProductPrice * item.OrderedCount

                    });


                }





            }
            MyModel.ShoppingCartViewModels = cartcontents;
            return View(MyModel);

        }



        #endregion
    }
}