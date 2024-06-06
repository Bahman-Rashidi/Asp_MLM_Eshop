using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Models;
using MLM_app.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MLM_app.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
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

        // GET: ShoppingCart

        #region Display Shopping Cart
        public ActionResult Index()
        {
            // Display contents of the shopping cart
            List<ShoppingCartViewModel> cartcontents = new List<ShoppingCartViewModel>();

            if (Session["ShoppingCartItems"] != null)
            {
                // Here we used the var type
                List<ProductInShoppingCart> productsInCart = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;

                var dbProducts = db.Products.ToList();

                // Joined session with the products table here
                cartcontents = (from pr in dbProducts
                                join pic in productsInCart
                                    on pr.ProductID equals pic.ProductID
                                select new ShoppingCartViewModel
                                {
                                    ProductID = pr.ProductID,
                                    ProductTitle = pr.ProductTitle,
                                    ProductPrice = (pr.ProductPrice - (pr.ProductPrice * pr.Off / 100)),
                                    ProductCount = pic.ProductCount,
                                    RowTotal = (pr.ProductPrice - (pr.ProductPrice * pr.Off / 100)) * pic.ProductCount,
                                    Off = pr.Off
                                }).ToList();
                return View(cartcontents);
            }
            return View(cartcontents);
        }
        #endregion

        #region Decrease Quantity of Items in Shopping Cart
        public ActionResult SubCount(int id)
        {
            if (Session["ShoppingCartItems"] != null)
            {
                List<ProductInShoppingCart> productsInCart = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;

                int index = productsInCart.FindIndex(p => p.ProductID == id);

                ProductInShoppingCart item = productsInCart[index];
                if (item.ProductCount == 1)
                {
                    productsInCart.RemoveAt(index);
                }
                else
                {
                    item.ProductCount--;
                    productsInCart[index] = item;
                }
                Session["ShoppingCartItems"] = productsInCart;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Adding a certain quantity of items from the shopping cart

        public ActionResult AddCount(int id)
        {
            if (Session["ShoppingCartItems"] != null)
            {
                List<ProductInShoppingCart> productsInCart = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;

                int index = productsInCart.FindIndex(p => p.ProductID == id);

                ProductInShoppingCart item = productsInCart[index];
                item.ProductCount++;
                productsInCart[index] = item;
                Session["ShoppingCartItems"] = productsInCart;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Remove Item from Shopping Cart
        public ActionResult Delete(int id)
        {
            if (Session["ShoppingCartItems"] != null)
            {
                List<ProductInShoppingCart> productsInCart = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;
                int index = productsInCart.FindIndex(p => p.ProductID == id);
                productsInCart.RemoveAt(index);
                Session["ShoppingCartItems"] = productsInCart;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Finalizing Order of Items and Submitting to Database
        [Authorize]
        // Finalizing Order User 
        public ActionResult Finalize()
        {
            List<ShoppingCartViewModel> cartcontents = new List<ShoppingCartViewModel>();

            if (Session["ShoppingCartItems"] != null)
            {
                List<ProductInShoppingCart> productsInCart = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;

                var dbProducts = db.Products.ToList();

                cartcontents = (from pr in dbProducts
                                join pic in productsInCart on pr.ProductID equals pic.ProductID
                                select new ShoppingCartViewModel
                                {
                                    ProductID = pr.ProductID,
                                    ProductTitle = pr.ProductTitle,
                                    //ProductPrice = (pr.ProductPrice - (pr.ProductPrice * pr.Off / 100)),
                                    ProductPrice = pr.ProductPrice,
                                    ProductCount = pic.ProductCount,
                                    RowTotal = (pr.ProductPrice - (pr.ProductPrice * pr.Off / 100)) * pic.ProductCount,
                                    Off = pr.Off,
                                }).ToList();

                var userId = User.Identity.GetUserId();

                // Submit Order 
                Order order = new Order()
                {
                    ApplicationUserId = userId,
                    OrderDate = DateTime.Now,
                    IsFinalized = false,
                    ReciptionMethod = "Unknown"
                };

                db.Orders.Add(order);

                // Submitting the order to the OrderDetails table
                foreach (ShoppingCartViewModel item in cartcontents)
                {
                    db.OrderDetails.Add(new OrderDetail()
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        OrderedCount = item.ProductCount,
                        Off = item.Off,
                        Price = item.ProductPrice
                    });
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                 .SelectMany(x => x.ValidationErrors)
                 .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }


                Session["ShoppingCartItems"] = null;
            }

            return RedirectToAction("Orders");
        }
        #endregion

        #region Order 
        [Authorize]
        public ActionResult Orders()
        {
            var userId = User.Identity.GetUserId();

            var userOrders = (from o in db.Orders
                              where o.ApplicationUserId == userId
                              select new OrdersViewModel()
                              {
                                  OrderID = o.OrderID,
                                  OrderDate = o.OrderDate,
                                  ReciptionMethod = o.ReciptionMethod,
                                  Price = o.ReciptionPrice,
                                  IsFinalized = o.IsFinalized,
                                  // Here we use a subquery to calculate the total sum of the amount
                                  OrderTotal = (from od in db.OrderDetails
                                                join p in db.Products on od.ProductID equals p.ProductID
                                                where od.OrderID == o.OrderID
                                                select (od.Price - (od.Price * od.Off / 100)) * od.OrderedCount).Sum(),
                                  OrderTotalWithReception = (from od in db.OrderDetails
                                                             join p in db.Products on od.ProductID equals p.ProductID
                                                             where od.OrderID == o.OrderID
                                                             select (od.Price - (od.Price * od.Off / 100)) * od.OrderedCount).Sum() +o.ReciptionPrice,

                                  // OrderDetails is a list of sub-factors
                                  OrderDetails = o.OrderDetails
                              }).OrderByDescending(x => x.OrderDate).ToList();


            return View(userOrders);
        }


    
        #endregion

        #region


        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            UserViewModel user = new UserViewModel();
            var userId = User.Identity.GetUserId();
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(userId);
            applicationUser.PasswordHash = null;
            user.Address = applicationUser.Address;
            user.Email = applicationUser.Email;
            user.PhoneNumber = applicationUser.PhoneNumber;
            user.ZipCode = applicationUser.ZipCode;

            if (string.IsNullOrEmpty(user.Address) && string.IsNullOrEmpty(user.PhoneNumber) && string.IsNullOrEmpty(user.ZipCode))
            {
                user.ValidCOntractInfo = true;
            }

            return Json(user, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<ActionResult> GetUserProfile(string userId)
        {

            UserViewModel user = new UserViewModel();
            // var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                ApplicationUser applicationUser = await UserManager.FindByIdAsync(userId);
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


        #region Invoice Details

        public ActionResult details(int id)
        {

            MyOrdersViewModel MyModel = new MyOrdersViewModel();
            List<ShoppingCartViewModel> cartcontents = new List<ShoppingCartViewModel>();


            var order = db.Orders.FirstOrDefault(x => x.OrderID == id);
            if (order != null)
            {
                MyModel.ReciptionMethod = order.ReciptionMethod;

             var orderq=   db.ReciptionMethods.FirstOrDefault(x => x.EnReceptionMethodName == order.ReciptionMethod);
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

        #region Delete Factor

        public ActionResult deletea(int id)
        {


            var qx = db.OrderDetails.Where(x => x.OrderID == id);
            if (qx != null)
            {
                //  db.OrderDetails.Where(x => x.OrderID == id).Delete();
                foreach (var item in qx)
                {


                    db.OrderDetails.Attach(item);
                    db.OrderDetails.Remove(item);



                }
                db.SaveChanges();




            }

            var q2 = db.Orders.FirstOrDefault(x => x.OrderID == id);

            db.Orders.Attach(q2);
            db.Orders.Remove(q2);
            db.SaveChanges();



            var userId = User.Identity.GetUserId();
            List<OrdersViewModel> userOrdersq2 = new List<OrdersViewModel>();

            var q = db.Orders.Where(x => x.ApplicationUserId == userId && x.IsFinalized == false);

            if (q != null)
            {
                foreach (var o in q)
                {
                    var LitOrder = db.OrderDetails.Where(x => x.OrderID == o.OrderID).GroupBy(x => x.OrderID);
                    int OrdPice = 0;
                    foreach (var iten in LitOrder)
                    {

                        var dd = iten.Key;

                        foreach (var item in iten)
                        {

                            OrdPice += (item.Price * item.OrderedCount);

                        }


                    }


                    var newO = new OrdersViewModel();


                    newO.OrderID = o.OrderID;
                    newO.OrderDate = o.OrderDate;
                    newO.IsFinalized = o.IsFinalized;
                    newO.OrderTotal = OrdPice;

                    userOrdersq2.Add(newO);


                }

            }

            return View("Orders", userOrdersq2);

        }

        #endregion
        #region ReciptionMethod

        [HttpGet]
        public ActionResult ReciptionMethod(int id)
        {

            var MyOrder = new Order();
            var q = db.Orders.FirstOrDefault(x => x.OrderID == id);

            if (q != null)
            {
                MyOrder = q;



            }


            ViewBag.ReciptionMethods = db.ReciptionMethods.Select(x => x).ToList();
            return View(q);
        }

        [HttpPost]
        public ActionResult ReciptionMethod(Order obj)
        {

           var q= db.Orders.FirstOrDefault(x => x.OrderID == obj.OrderID);
            if (q != null)
            {

                q.ReciptionMethod = obj.ReciptionMethod;

                var p = db.ReciptionMethods.FirstOrDefault(x => x.EnReceptionMethodName == obj.ReciptionMethod);
                if (p != null)
                {
                    q.ReciptionPrice = p.Price;

                }

                db.SaveChanges();
            }
           
         //   return View("Orders");
           return RedirectToAction("Orders");
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