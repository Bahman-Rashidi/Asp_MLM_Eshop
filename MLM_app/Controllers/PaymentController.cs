using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;
using MLM_app.Models;
/******************************************/
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MLM_app.Controllers
{
    [Authorize]
    public class PaymentController : Controller
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

        #region Online Payment
        public ActionResult BankPayPayment(int id)
        {
            try
            {

                var q = db.Orders.FirstOrDefault(x => x.OrderID == id).ReciptionPrice;
                // With this command, we obtain the total payment amount
                int orderTotal = (from od in db.OrderDetails
                                  where od.OrderID == id
                                  select (od.Price - (od.Price * od.Off / 100))).Sum();

                ViewBag.Price = orderTotal+q;
                ViewBag.Id = id;

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        #endregion

        #region Bank connection action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BankPayPayment(int id, string price)
        {
            if (price != null)
            {
                var userId = User.Identity.GetUserId();
                Payment payment = new Payment() { OrderId = id, Amount = Int64.Parse(price), InsertDateTime = DateTime.Now, SaleReferenceId = 123,ApplicationUserId = userId};
                db.Payments.Add(payment);
                db.SaveChanges();                

                long saleOrderId = payment.PaymentId;

                string MyDomain = "http://www.test.com";

                var description = "test";

                var returnurl = MyDomain + "/Payment/BankPayCallBack";

                decimal Price = decimal.Parse(price);


                long OrderId = (long)GetUniqueKey(8);

                BankPayPayment(returnurl, description, Price, saleOrderId);
            }
            else
            {
                ViewBag.message = "Please enter the required information.";
            }

            return View();
        }
        #endregion

        #region // Security message dismissal function
        void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate(
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }
        #endregion

        #region // The function to receive date
        protected string GetDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
        }
        #endregion

        #region // The function to receive time
        protected string GetTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0');
        }
        #endregion

        #region // Unique code creation function

        protected int GetUniqueKey(int max)
        {
            int maxSize = max;
            var chars = new char[62];
            const string a = "1234567890";

            chars = a.ToCharArray();

            int size = maxSize;
            var data = new byte[1];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                var result = new StringBuilder(size);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length - 1)]);
                }
                return int.Parse(result.ToString());
            }
        }

        #endregion

        #region // The function to retrieve the address of the Bank 
        protected string GetPgwSite()
        {
            return ConfigurationManager.AppSettings["PgwSite"].TrimEnd();
        }
        #endregion

        #region // The function to call the bank payment request
        protected void BankPayPayment(string returnurl, string description, decimal Price, long OrderId)
        {

            try
            {

                
                BypassCertificateError();


                string TerminalId = ConfigurationManager.AppSettings["TerminalId"];

                string UserName = ConfigurationManager.AppSettings["UserName"];

                string UserPassword = ConfigurationManager.AppSettings["UserPassword"];

             //   BankPayServiceReference.PaymentGatewayClient p = new BankPayServiceReference.PaymentGatewayClient();

                var Mydate = GetDate();
                var MyTime = GetTime();

                string result = "you paid ";



                if (result != null)
                {
                    String[] resultArray = result.Split(',');

                    if (int.Parse(result[0].ToString()) == 0)
                    {
                        ViewBag.jscode = "<script language='javascript' type='text/javascript'> postRefId('" + resultArray[1] + "');</script>";
                    }
                    else
                    {
                        ViewBag.message = Infrastructure.BankBankPayResult.ResultText(result);
                    }

                }
                else
                {
                    ViewBag.message = "Currently, connection to this gateway is not available.";
                }


            }
            catch (Exception exp)
            {
                ViewBag.message = "Error: " + exp.Message;
            }


        }
        #endregion

        #region CallBack
        [AllowAnonymous]
        public ActionResult BankPayCallBack(string RefId, string ResCode, string SaleOrderId, string SaleReferenceId)
        {
            CallBack();
            return View();
        }
        #endregion

        #region Bank Refund Verification Method
        private void CallBack()
        {
            // Calling the method to fix security messages
            BypassCertificateError();

            if (string.IsNullOrEmpty(Request.Params["SaleReferenceId"]))
            {
                if (!string.IsNullOrEmpty(Request.Params["ResCode"]))
                {
                    ViewBag.message = BankBankPayResult.ResultText(Request.Params["ResCode"]);
                }
                else
                {
                    ViewBag.message = "Status: Receipt number is not acceptable.";
                }
                ViewBag.Image = "~/Images/notaccept.png";
            }
            else
            {
                try
                {
                    string TerminalId = ConfigurationManager.AppSettings["TerminalId"];
                    string UserName = ConfigurationManager.AppSettings["UserName"];
                    string UserPassword = ConfigurationManager.AppSettings["UserPassword"];


                    long SaleOrderId = 0;
                    long SaleReferenceId = 0;
                    string RefId = null;

                    try
                    {
                        SaleOrderId = long.Parse(Request.Params["SaleOrderId"].TrimEnd());
                        SaleReferenceId = long.Parse(Request.Params["SaleReferenceId"].TrimEnd());
                        RefId = Request.Params["RefId"].TrimEnd();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.message = ex + "<br/>" + "Status: There is an issue with the payment. If the payment amount has been deducted from your bank account, it will be automatically refunded.";

                        ViewBag.Image = "~/Images/notaccept.png";
                        return;
                    }

                  //  BankPayServiceReference.PaymentGatewayClient bpService = new BankPayServiceReference.PaymentGatewayClient();
                    string Vresult;
                    //VerifyRequest
                    Vresult = "0";

                    if (!string.IsNullOrEmpty(Vresult))
                    {
                        if (Vresult == "0")
                        {
                            string IQresult;
                            IQresult = "0";
                            if (IQresult == "0")
                            {
                                //Payment has been made here and the Operation related to the website is being carried out

                                int paymentId = Convert.ToInt32(SaleOrderId);

                                SuccessPayment(paymentId, Vresult, SaleReferenceId, RefId);
                                ViewBag.message ="Payment has been successfully processed." + "<br/>" + "Order ID: " + SaleOrderId + "<br/>" + "Transaction Reference ID:" + SaleReferenceId + "<br/>" +"Payment receipt:" + RefId;

                                ViewBag.Image = "~/Images/accept.png";

                                // Final payment notification
                                string Sresult;
                                Sresult = "45";
                                {
                                    if (Sresult == "0" || Sresult == "45")
                                    {
                                        //The transaction has been confirmed and settled 
                                    }
                                    else
                                    {
                                        //The transaction has been confirmed but not settled
                                    }
                                }


                            }
                            else
                            {
                                // The operation of refunding the amount
                                string Rvresult;

                                //  Reversal Request function shpud be added 
                                Rvresult = " ";

                                ViewBag.message = "The transaction has been refunded";
                                ViewBag.Image = "~/Images/notaccept.png";

                                int paymentId = Convert.ToInt32(SaleOrderId);

                                FailedPayment(paymentId, Rvresult, SaleReferenceId, RefId);
                                
                            }

                        }
                        else
                        {
                            ViewBag.message = BankBankPayResult.ResultText(Vresult);
                            ViewBag.Image = "~/Images/notaccept.png";

                            int paymentId = Convert.ToInt32(SaleOrderId);

                            FailedPayment(paymentId, Vresult, SaleReferenceId, RefId);
                        }

                    }
                    else
                    {
                        ViewBag.message = "Status: The receipt number is not acceptable.";
                        ViewBag.Image = "~/Images/notaccept.png";
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.message = ex + "<br/>" + "Status: There is an issue with the payment. If the payment amount has been deducted from your bank account, it will be automatically refunded.";
                    ViewBag.Image = "~/Images/notaccept.png";
                }
            }
        }

        #endregion

        #region The successful payment save method

        private void SuccessPayment(int paymentId, string Vresult, long SaleReferenceId, string RefId)
        {
            var orderid = (from pu in db.Payments
                           where pu.PaymentId == paymentId
                           select pu.OrderId).FirstOrDefault();

            #region decrese  produtcs  amount
            //AvailableNumber
            var q = db.OrderDetails.Where(x => x.OrderID == orderid);
            if (q != null)
            {

                foreach (var orderDetails in q)
                {

                    var myProduct = db.Products.FirstOrDefault(x => x.ProductID == orderDetails.ProductID);
                    if (myProduct != null)
                    {
                        //.AvailableNumber=
                        myProduct.AvailableNumber = myProduct.AvailableNumber - orderDetails.OrderedCount;

                    }

                }

                db.SaveChanges();

            }


            #endregion

            var payment = db.Payments.Find(paymentId);

            payment.ResCode = Vresult;
            payment.SaleReferenceId = SaleReferenceId;
            payment.ReferenceNumber = RefId;



            //Order update as finalized.

            var orders = db.Orders.FirstOrDefault(o => o.OrderID == orderid);
            if (orders != null)
                orders.IsFinalized = true;
            var userId = User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(userId);
            //Update Last purchase User


            if (user != null)
            {

                user.LatestDateOfPurchase = DateTime.Now;
              
                user.AllowGetPeriodicalDividend = true;
                UserManager.Update(user);

                #region Profit-sharing with top users

                var Amount = payment.Amount;


                List<string> parentUseIds = new List<string>();
                #region Get Parent User

                if (user.P1 != null)
                {
                    parentUseIds.Add(user.P1);
                }
                if (user.P2 != null)
                {
                    parentUseIds.Add(user.P2);
                }
                if (user.P3 != null)
                {
                    parentUseIds.Add(user.P3);
                }
                if (user.P4 != null)
                {
                    parentUseIds.Add(user.P4);
                }
                if (user.P5 != null)
                {
                    parentUseIds.Add(user.P5);
                }
                if (user.P6 != null)
                {
                    parentUseIds.Add(user.P6);
                }
                if (user.P7 != null)
                {
                    parentUseIds.Add(user.P7);
                }
                if (user.P8 != null)
                {
                    parentUseIds.Add(user.P8);
                }
                if (user.P9 != null)
                {
                    parentUseIds.Add(user.P9);
                }
                if (user.P10 != null)
                {
                    parentUseIds.Add(user.P10);
                }
                #endregion

                var PerntUser = db.Users.Where(x => parentUseIds.Contains(x.Id));

                foreach (var Suser in PerntUser)
                {
                   
                        Suser.PeriodicalDividend = (long)(Suser.PeriodicalDividend + (Amount * (0.0125)));
               
                   

                }
                 


                #endregion

            }
            else
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(" Order Id = " + orderid + " user  after  payment is null !?!?! "));
               
            }
            payment.IsFinalized = true;
            payment.ApplicationUserId = userId;

            //LatestDateOfPurchase

            db.SaveChanges();
        }
        #endregion

        #region The unsuccessful payment save method
        private void FailedPayment(int SaleOrderId, string Rvresult, long SaleReferenceId, string RefId)
        {


            //  The request number 41 is duplicate, please try again
            int paymentId = Convert.ToInt32(SaleOrderId);

            var orderid = (from pu in db.Payments
                           where pu.PaymentId == paymentId
                           select pu.OrderId).FirstOrDefault();

            //IsDuplicatedBank
            #region MyRegion
            if (Rvresult == "41")
            {


                var firstOrDefault = db.Orders.FirstOrDefault(x => x.OrderID == orderid);
                if (firstOrDefault != null)
                    firstOrDefault.IsDuplicatedBank = true;
            }
            #endregion


            var payment = db.Payments.Find(paymentId);

            payment.ResCode = Rvresult;
            payment.SaleReferenceId = SaleReferenceId;
            payment.ReferenceNumber = RefId;

            db.SaveChanges();
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