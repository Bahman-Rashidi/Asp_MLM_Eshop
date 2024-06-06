using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class Payment
    {
        #region constructor
        public Payment()
        {
        }
        #endregion

        #region Properties

        [Display(Name = "Order number")]
        public int PaymentId { get; set; }

        [Display(Name = "Order number")]
        public long SaleOrderId { get; set; }

        [Display(Name = "Amount")]
        public long Amount { get; set; }

        [Display(Name = "Submit Date and Time")]
        public DateTime InsertDateTime { get; set; }
       
        [StringLength(150)]

        [Display(Name = "Transaction number")]
        public string ReferenceNumber { get; set; }
        [StringLength(10)]

        [Display(Name = "Payment status")]
        public string ResCode { get; set; }

        [Display(Name = "Payment Order number")]
        public long? SaleReferenceId { get; set; }

        [Display(Name = "status Order")]
        public bool IsFinalized { get; set; }

        [Display(Name = "User placing the Order")]
        public string ApplicationUserId { get; set; }


        [Display(Name = "Profit-sharing status")]
        public bool IsDispachedDivide { get; set; }

        public int OrderId { get; set; }
        #endregion

        #region Relations
        //Navigation Properties
        public virtual Order Orders { get; set; }
        #endregion
    }
}