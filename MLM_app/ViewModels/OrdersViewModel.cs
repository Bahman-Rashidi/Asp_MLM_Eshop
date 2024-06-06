using MLM_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    public class OrdersViewModel
    {
        [Key]
        [Display(Name = "Invoice number")]
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name = "Invoice date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Paid")]
        public bool IsFinalized { get; set; }

        [Display(Name = "Invoice amount")]
        public int OrderTotal { get; set; }


        [Display(Name = "Payable")]
        public int OrderTotalWithReception { get; set; }

        [Display(Name = "Call cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [UIHint("Integer")]
        public int ContactPrice { get; set; }

        [Display(Name = "Shipping cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [UIHint("Integer")]
        public int Price { get; set; }

        [Display(Name = "Packing cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [UIHint("Integer")]
        public int packingPrice { get; set; }

        [Display(Name = "Method of receipt")]
        public string ReciptionMethod { get; set; }

        [Display(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "SaleReferenceId")]
        public long? SaleReferenceId { get; set; }


        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }


    public class MyOrdersViewModel
    {
        [Key]
        [Display(Name = "Invoice number")]
        public int OrderID { get; set; }

        public int UserID { get; set; }

        [Display(Name = "Invoice date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Paid")]
        public bool IsFinalized { get; set; }

        [Display(Name = "Total invoice amount")]
        public int OrderTotal { get; set; }


        [Display(Name = "Method of receipt")]
        public string ReciptionMethod { get; set; }


        [Display(Name = "Shipping cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [UIHint("Integer")]
        public int Price { get; set; }

        public virtual ICollection<ShoppingCartViewModel> ShoppingCartViewModels { get; set; }
    }

}